namespace User.API.Features.User.UpdateUser
{
    public record UpdateUserResponse(int Id, string Name, string Email,
        DateTime CreatedAt, DateTime UpdatedAt, byte IsActive); 
}
