namespace Notification.API.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; } = string.Empty; 
        public string Title { get; set; } = string.Empty; 
        public string Message { get; set; } = string.Empty;

        public int? RelatedEntityId { get; set; }
        public byte IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
