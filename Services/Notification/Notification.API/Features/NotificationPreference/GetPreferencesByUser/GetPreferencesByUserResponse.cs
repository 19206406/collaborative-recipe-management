namespace Notification.API.Features.NotificationPreference.GetPreferencesByUser
{
    public record GetPreferencesByUserResponse(int Id, int UserId, byte EmailNotifications, byte PushNotifications); 
}
