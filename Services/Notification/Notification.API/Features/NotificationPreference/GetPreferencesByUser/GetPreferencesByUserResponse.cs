namespace Notification.API.Features.NotificationPreference.GetPreferencesByUser
{

    public record PreferencesResponse(int Id, byte EmailNotifications, byte PushNotifications);
    public record GetPreferencesByUserResponse(List<PreferencesResponse> notificationPreferences); 
}
