namespace Notification.API.Features.Notification.MarkAsRead
{
    public record MarkAsReadResponse(
        int Id, int UserId, string Type,
        string Title, string Message,
        byte IsRead, DateTime CreatedAt); 
}
