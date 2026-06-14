namespace Notification.API.Features.Notification.GetNumberOfNotificationsByUser
{
    public record GetNumberOfNotificationsByUserResponse(int Id, string Type, string? Message, string Title, DateTime CreatedAt); 
}
