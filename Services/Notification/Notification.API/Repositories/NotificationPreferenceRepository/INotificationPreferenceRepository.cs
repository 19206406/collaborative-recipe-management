using Notification.API.Entities;

namespace Notification.API.Repositories.NotificationPreferenceRepository
{
    public interface INotificationPreferenceRepository
    {
        Task<NotificationPreference?> GetPreferencesByUserIdAsync(int userId);
        Task UpdatePreferencesByUserIdAsync();
        Task CreatePreferencesByUserAsync(Entities.NotificationPreference userPreferences); 
    }
}
