namespace Notification.API.Repositories.NotificationRepository
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Entities.Notification>> GetNotificationsByUserIdAsync(int userId); 
    }
}
