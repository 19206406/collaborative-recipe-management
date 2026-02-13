using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace BuildingBlocks.Messaging.RabbitMQ
{
    public abstract class RabbitMQConsumer<T> : BackgroundService where T : class
    {
        protected readonly ILogger<RabbitMQConsumer<T>> Logger;
        private readonly RabbitMQSettings _settings;
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly JsonSerializerOptions _jsonOptions; 

        // nombre de la cola a consumir (debe ser definido por clases hijas) 
        protected abstract string QueueName { get; }

        // prefetch count - cantidad de mensajes a procesar en paralelo 
        // por defecto 1 - prcesa un mensaje a la vez 
        protected virtual ushort PrefetchCount => 1;

        // define si la cola debe ser durable 
        protected virtual bool QueueDurable => true; 

        protected RabbitMQConsumer(
            IOptions<RabbitMQSettings> settings, 
            ILogger<RabbitMQConsumer<T>> logger)
        {
            _settings = settings.Value;
            Logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            }; 
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested(); 

            try
            {
                await InitializeRabbitMQAsync(cancellationToken: stoppingToken);

                // mantener el servicio en ejecución 
                await Task.Delay(Timeout.Infinite, stoppingToken); 


            }
            catch (OperationCanceledException)
            {
                Logger.LogInformation("Consumidor detenido: {Queue}", QueueName); 
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error al inicializar consumidor para cola '{Queue}'", QueueName);
                throw; 
            }
        }

        private async Task InitializeRabbitMQAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                // ConsumerDispatchConcurrency = true  // Importante para async/await
            };

            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            // declarar la cola 
            await _channel.QueueDeclareAsync(
                queue: QueueName,
                durable: QueueDurable,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            await _channel.BasicQosAsync(
                prefetchSize: 0,
                prefetchCount: PrefetchCount,
                global: false,
                cancellationToken: cancellationToken);

            Logger.LogInformation(
                "Consumidor iniciado. Esperando mensajes en cola: '{Queue}",
                QueueName);

            // Configurar el consumidor asincrono 
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += OnMessageReceivedAsync;

            await _channel.BasicConsumeAsync(
                queue: QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: cancellationToken); 
        }

        private async Task OnMessageReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
        {
            var messageId = eventArgs.BasicProperties?.MessageId ?? Guid.NewGuid().ToString();

            try
            {
                var body = eventArgs.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);

                Logger.LogInformation(
                "Mensaje recibido de '{Queue}' - ID: {MessageId}",
                QueueName,
                messageId);

                Logger.LogDebug("Contenido: {Menssage}", messageJson);

                var message = JsonSerializer.Deserialize<T>(messageJson, _jsonOptions); 

                if (message is null)
                {
                    Logger.LogWarning("Mensaje null despues de deserailizar. Rechanzando mensaje.");
                    await _channel!.BasicNackAsync(eventArgs.DeliveryTag, false, false);
                    return; 
                }

                // procesar el mensaje 
                await ProcessMessageAsync(message);

                // hacer ack del mensaje (confirmar que se procesó exitosamente)    
                await _channel!.BasicAckAsync(eventArgs.DeliveryTag, false);

                Logger.LogInformation(
                    "Mensaje procesado exitosamente - ID: {MessageId}", messageId); 
            } 
            catch (JsonException jsonEx)
            {
                Logger.LogError(jsonEx, "Error al deserializar mensaje - ID: {MessageId}. Mensaje rechazado.", messageId);

                // rechazar sin requeue (mensaje malformado no se puede procesar) 
                await _channel!.BasicNackAsync(eventArgs.DeliveryTag, false, false); 
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error al procesar mensaje - ID: {MessageId}", messageId);

                // rechazar con requeue (intentar procesar nuevamente)
                await _channel!.BasicNackAsync(eventArgs.DeliveryTag, false, true); 
            }
        }

        // metodo que implementa o contiene la logica de procesamiento del mensaje 
        protected abstract Task ProcessMessageAsync(T message); 

        public override async void Dispose()
        {
            try
            {
                if (_channel is not null)
                {
                    await _channel.CloseAsync();
                    _channel.Dispose(); 
                } 

                if (_connection is not null)
                {
                    await _connection.CloseAsync();
                    _connection.Dispose();
                }

                Logger.LogInformation("Consumidor detenido y conexión cerradoa: '{Queue}'", QueueName); 
            } 
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error al cerrar consumidor"); 
            }
        }
    }
}
