namespace Notification.API.Entities
{
    public class NotificationPreference
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public byte EmailNotifications { get; set; }
        public byte PushNotifications { get; set; }
    }
}
