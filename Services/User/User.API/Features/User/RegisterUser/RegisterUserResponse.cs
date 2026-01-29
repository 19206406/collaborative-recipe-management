namespace User.API.Features.User.RegisterUser
{
    public record RegisterUserResponse(int Id, string Name, string Email,
        DateTime CreatedAt, DateTime UpdatedAt, byte IsActive); 
}
