namespace BuildingBlocks.Messaging.RabbitMQ
{
    public class RabbitMQSettings
    {
        public const string SectionName = "RabbitMQ"; 

        // hostname del servidor rabbitmq 
        public string HostName { get; set; }

        // puerto AMQP (por defecto 5672) 
        public int Port { get; set; } = 5672;

        // usuario de RabbitMQ 
        public string UserName { get; set; } = "guest";

        public string Password { get; set; } = "guest";

        public string VirtualHost { get; set; } = "/";

        public int RequestedConnectionTimeout { get; set; } = 30000;

        public int RetryCount { get; set; } = 5; 
    }
}
