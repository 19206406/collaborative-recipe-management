using Notification.API.Features.NotificationPreference.GetPreferencesByUser;

namespace Notification.API.Features.NotificationPreference.UpdatePreferencesByUser
{
    public record UpdatePreferencesByUserResponse(List<PreferencesResponse> notificationPreferences); 
}
