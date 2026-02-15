namespace BuildingBlocks.Messaging.Events
{
    // clase base para los eventos de integración 
    public abstract class IntegrationEvent
    {
        public Guid EventId { get; set; } = Guid.NewGuid(); 
        public DateTime CreatedAt { get; set; }
        public string EventType => GetType().Name; 
    }

    public interface IDomainEvent
    {
        Guid DomainId { get; set; }
        DateTime OcurredAt { get; set; }
    }
}
