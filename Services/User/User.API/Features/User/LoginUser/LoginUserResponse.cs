namespace User.API.Features.User.LoginUser
{
    public record UserLogin(int Id, string Name, string Email, DateTime CreatedAt, byte IsActive); 
    public record LoginUserResponse(string AccessToken, string RefreshToken, UserLogin User); 
}
