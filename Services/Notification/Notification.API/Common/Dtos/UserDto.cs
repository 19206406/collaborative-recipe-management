namespace Notification.API.Common.Dtos
{
    public record UserDto(int Id, string Name, string Email,
        DateTime CreatedAt, DateTime UpdatedAt, byte IsActive);
}
