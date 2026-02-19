using Notification.API.Entities;

namespace Notification.API.Repositories.NotificationPreferenceRepository
{
    public interface INotificationPreferenceRepository
    {
        Task<IEnumerable<NotificationPreference>> GetPreferencesByUserIdAsync(int userId);
        Task<bool> UpdatePreferencesByUserIdAsync(List<NotificationPreference> notificationPreferences); 
    }
}
