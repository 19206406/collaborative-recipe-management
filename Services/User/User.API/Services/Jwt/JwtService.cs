using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BuildingBlocks.Jwt.Models;

namespace User.API.Services.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings; // clase que guarda todos los campos de la configuración 

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateAccessToken(Entities.User user)
        {
            // lista de los elementos a guardar 
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
                new Claim("userId", user.Id.ToString()), 
                new Claim("name", user.Name),  
            };

            // agregar cada audiencia como un claim individual 
            foreach (var audience in _jwtSettings.Audiences)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience)); 
            }

            // llave y la credencial del token 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // armado del token 
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: credentials);

            // escritura y retorno del token 
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber); 
        }

        public int GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        public ClaimsPrincipal ValidateToken(string token, bool validateLifeTime = true)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey); 

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudiences = _jwtSettings.Audiences,
                    ValidateLifetime = validateLifeTime,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                return principal; 
            }
            catch
            {
                return null; 
            }
        }
    }
}
