namespace User.API.Features.User.GetUser
{
    public record GetUserResponse(
        int Id, string Name, string Email, 
        DateTime CreatedAt, DateTime UpdatedAt, byte IsActive); 
}
