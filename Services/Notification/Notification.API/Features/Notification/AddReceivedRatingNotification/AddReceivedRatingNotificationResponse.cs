namespace Notification.API.Features.Notification.AddReceivedRatingNotification
{
    public record AddReceivedRatingNotificationResponse(int Id, int UserId, string Type, string Title,
        string Message, byte IsRead, DateTime CreatedAt); 
}
