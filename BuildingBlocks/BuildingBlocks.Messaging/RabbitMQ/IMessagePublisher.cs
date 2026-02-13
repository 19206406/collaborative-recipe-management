namespace BuildingBlocks.Messaging.RabbitMQ
{
    public interface IMessagePublisher
    {
        // publica un mensaje a una cola específica 
        // tipo del mensaje 
        // nombre de la cola destino 
        // mensaje a publicar 
        Task PublishAsync<T>(string queueName, T message, CancellationToken cancellationToken = default) where T : class;

        // publica un mensaje a un exchange con routing key 
        // tipo del mensaje 
        // nombre del exchange
        // routing key para el mensaje 
        // mensaje a publicar 
        Task PublishToExchangeAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default) where T : class; 
    }
}
