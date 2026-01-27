using System.Security.Claims;

namespace User.API.Services.Jwt
{
    public interface IJwtService
    {
        string GenerateAccessToken(Entities.User user);
        string GenerateRefreshToken();
        ClaimsPrincipal ValidateToken(string token, bool validateLifeTime = true);
        int GetUserIdFromToken(string token); 
    }
}
