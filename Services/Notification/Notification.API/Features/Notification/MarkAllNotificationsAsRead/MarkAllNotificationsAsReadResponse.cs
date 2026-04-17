namespace Notification.API.Features.Notification.MarkAllNotificationsAsRead
{
    public record NotificationResponse(int Id, int UserId, string Type, string Title);

    public record MarkAllNotificationsAsReadResponse(List<NotificationResponse> Notifications); 
}
