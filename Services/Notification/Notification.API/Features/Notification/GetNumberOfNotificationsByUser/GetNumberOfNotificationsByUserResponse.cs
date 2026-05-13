namespace Notification.API.Features.Notification.GetNumberOfNotificationsByUser
{
    public record NotificationResponse(int Id, string Type, string? Message, string Title, DateTime CreatedAt); 
    public record GetNumberOfNotificationsByUserResponse(List<NotificationResponse> Notifications); 
}
