namespace Notification.API.Features.Notification.GetNumberOfNotificationsByUser
{
    public record NotificationResponse(int Id, string Type, string Title, DateTime CreatedAt); 
    public record GetNumberOfNotificationsByUserResponse(List<NotificationResponse> Notifications); 
}
