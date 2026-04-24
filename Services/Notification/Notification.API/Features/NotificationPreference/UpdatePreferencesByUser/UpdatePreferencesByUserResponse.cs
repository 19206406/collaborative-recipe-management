using Notification.API.Features.NotificationPreference.GetPreferencesByUser;

namespace Notification.API.Features.NotificationPreference.UpdatePreferencesByUser
{
    public record UpdatePreferencesByUserResponse(int Id, int UserId, byte EmailNotifications, byte PushNotifications); 
}
