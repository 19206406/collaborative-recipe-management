namespace Notification.API.Features.Notification.CreateAuxiliaryNotification
{
    public record CreateAuxiliaryNotificationResponse(int Id, int UserId, string Type, string Title, 
        string Message, byte IsRead, DateTime CreatedAt); 
}
