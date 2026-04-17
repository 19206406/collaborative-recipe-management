namespace Notification.API.Repositories.NotificationRepository
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Entities.Notification>> GetNotificationsByUserIdAsync(int userId);
        Task<IEnumerable<Entities.Notification>> GetNumberOfNotificationsUnReadByUserIdAsync(int userId); 
        Task<Entities.Notification?> GetNotificationByIdAsync(int id);
        Task<Entities.Notification> UpdateNotificationAsync(Entities.Notification notification); 
        Task<bool> UpdateAllNotificationsAsync(IEnumerable<Entities.Notification> notifications);
        Task UpdateNotificationsAsync();
        Task<Entities.Notification> AddNotificationAsync(Entities.Notification notification); 
    }
}
