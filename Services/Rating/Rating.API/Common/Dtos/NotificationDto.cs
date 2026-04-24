namespace Rating.API.Common.Dtos
{
    public record NotificationDto(int Id, int UserId, string Type, string Title,
        string Message, byte IsRead, DateTime CreatedAt); 
}
