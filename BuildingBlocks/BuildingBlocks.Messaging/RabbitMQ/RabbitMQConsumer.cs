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

        protected abstract string QueueName { get; }
        protected virtual ushort PrefetchCount => 1;
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
                await InitializeRabbitMQAsync(stoppingToken);
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
                ConsumerDispatchConcurrency = PrefetchCount  // ✅ ushort, no bool
            };

            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

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

            Logger.LogInformation("Consumidor iniciado. Esperando mensajes en cola: '{Queue}'", QueueName);

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

                Logger.LogInformation("Mensaje recibido de '{Queue}' - ID: {MessageId}", QueueName, messageId);
                Logger.LogDebug("Contenido: {Message}", messageJson);

                var message = JsonSerializer.Deserialize<T>(messageJson, _jsonOptions);

                if (message is null)
                {
                    Logger.LogWarning("Mensaje null después de deserializar. Rechazando mensaje.");
                    await _channel!.BasicNackAsync(eventArgs.DeliveryTag, false, false);
                    return;
                }

                await ProcessMessageAsync(message);
                await _channel!.BasicAckAsync(eventArgs.DeliveryTag, false);

                Logger.LogInformation("Mensaje procesado exitosamente - ID: {MessageId}", messageId);
            }
            catch (JsonException jsonEx)
            {
                Logger.LogError(jsonEx, "Error al deserializar mensaje - ID: {MessageId}. Mensaje rechazado.", messageId);
                await _channel!.BasicNackAsync(eventArgs.DeliveryTag, false, false);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error al procesar mensaje - ID: {MessageId}", messageId);
                await _channel!.BasicNackAsync(eventArgs.DeliveryTag, false, false); // cambio a false 
            }
        }

        protected abstract Task ProcessMessageAsync(T message);

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);

            try
            {
                if (_channel?.IsOpen == true)
                    await _channel.CloseAsync(cancellationToken);

                if (_connection?.IsOpen == true)
                    await _connection.CloseAsync(cancellationToken);

                Logger.LogInformation("Consumidor detenido y conexión cerrada: '{Queue}'", QueueName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error al cerrar consumidor");
            }
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}