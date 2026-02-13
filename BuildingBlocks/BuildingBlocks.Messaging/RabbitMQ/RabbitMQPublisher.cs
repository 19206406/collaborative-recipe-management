using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging; 
using System.Text.Json;
using System.Text;

namespace BuildingBlocks.Messaging.RabbitMQ
{
    public class RabbitMQPublisher : IMessagePublisher, IDisposable
    {
        private IConnection? _connection;
        private IChannel? _channel; 
        private readonly ILogger<RabbitMQPublisher> _logger;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly RabbitMQSettings _settings; 
        private bool _disposed; 

        public RabbitMQPublisher(
            IOptions<RabbitMQSettings> settings, 
            ILogger<RabbitMQPublisher> logger)
        {
            _logger = logger;
            _settings = settings.Value; 

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
        }

        private async Task EnsureConnectionAsync()
        {
            if (_connection is not null && _connection.IsOpen && _channel is not null && _channel.IsOpen)
                return;

            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost,
                RequestedConnectionTimeout = TimeSpan.FromMilliseconds(_settings.RequestedConnectionTimeout),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync(); 

            _logger.LogInformation("Conexión establecida a RabbitMQ en {Host}:{Port}", _settings.HostName, _settings.Port);
        }


        public async Task PublishAsync<T>(string queueName, T message, CancellationToken cancellationToken = default) where T : class
        {

            await EnsureConnectionAsync(); 

            try
            {
                // declarar la cola si no existe se crea 
                await _channel.QueueDeclareAsync(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: cancellationToken);

                // serializar el mensaje
                var messageJson = JsonSerializer.Serialize(message, _jsonOptions);
                var body = Encoding.UTF8.GetBytes(messageJson);

                // configurar propiedades del mensaje 
                var properties = new BasicProperties
                {
                    Persistent = true, // el mensaje sobrevive a reinicios del broker 
                    ContentType = "application/json",
                    Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                };

                // publicar el mensaje en la cola 
                await _channel.BasicPublishAsync(
                    exchange: string.Empty, // publicar directamente a la cola
                    routingKey: queueName,
                    mandatory: false,
                    basicProperties: properties,
                    body: body,
                    cancellationToken: cancellationToken);

                _logger.LogInformation("Mensaje publicado a cola '{Queue}': {MessageType}", queueName, typeof(T).Name);

                _logger.LogDebug("Contenido {Message}", messageJson); 
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al publicar mensaje a cola '{Queue}'", queueName);
                throw; 
            }
        }

        public async Task PublishToExchangeAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                await _channel.ExchangeDeclareAsync(
                    exchange: exchange,
                    type: ExchangeType.Topic,
                    durable: true,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: cancellationToken);

                // serializar el mensaje
                var messageJson = JsonSerializer.Serialize(message, _jsonOptions);
                var body = Encoding.UTF8.GetBytes(messageJson);

                // configurar propiedades del mensaje 
                var properties = new BasicProperties
                {
                    Persistent = true, // el mensaje sobrevive a reinicios del broker 
                    ContentType = "application/json",
                    Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                };

                // publicar el mensaje en la cola 
                await _channel.BasicPublishAsync(
                    exchange: exchange, // publicar directamente a la cola
                    routingKey: routingKey,
                    mandatory: false,
                    basicProperties: properties,
                    body: body,
                    cancellationToken: cancellationToken);

                _logger.LogInformation(
                "Mensaje publicado a exchange '{Exchange}' con routing key '{RoutingKey}': {MessageType}",
                exchange,
                routingKey,
                typeof(T).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                "Error al publicar mensaje a exchange '{Exchange}' con routing key '{RoutingKey}'",
                exchange,
                routingKey);
                throw;
            }
        }

        public void Dispose()
        {
            if (_disposed)
                return; 

            try
            {
                _channel?.CloseAsync().GetAwaiter().GetResult();
                _channel?.Dispose();
                _connection?.CloseAsync().GetAwaiter().GetResult();
                _connection?.Dispose();
                _logger.LogInformation("Conexión a RabbitMQ cerrada"); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cerrar conexión RabbitMQ"); 
            }

            _disposed = true; 
        }
    }
}
