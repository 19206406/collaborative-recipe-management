namespace Notification.API.Features.Notification.GetNotificationsByUser
{
    public record GetNotificationsByUserResponse(int Id, int UserId, string Type, string Title, string Message,
        byte IsRead, DateTime CreatedAt); 
}
