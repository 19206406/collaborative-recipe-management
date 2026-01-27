namespace User.API.Features.User.LoginUser
{
    public record LoginUserResponse(int Id, string Name, string Email,
        DateTime CreatedAt, byte IsActive); 
}
