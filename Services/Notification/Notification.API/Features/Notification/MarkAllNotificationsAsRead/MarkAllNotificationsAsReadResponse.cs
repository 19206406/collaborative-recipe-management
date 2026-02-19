using Notification.API.Features.Notification.GetNotificationsByUser;

namespace Notification.API.Features.Notification.MarkAllNotificationsAsRead
{
    public record MarkAllNotificationsAsReadResponse(List<NotificationResponse> Notifications); 
}
