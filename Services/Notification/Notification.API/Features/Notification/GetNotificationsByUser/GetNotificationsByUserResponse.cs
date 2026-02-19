namespace Notification.API.Features.Notification.GetNotificationsByUser
{
    public record NotificationResponse(
        int Id, int UserId, string Type, 
        string Title, string Message, 
        byte IsRead, DateTime CreatedAt); 

    public record GetNotificationsByUserResponse(List<NotificationResponse> Notifications); 
}
