using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ApiGateway.Middleware
{
    public class GatewayJwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RsaSecurityKey _rsaPublicKey; 
        private readonly string _issuer;
        private readonly List<string> _audiences;

        private record PublicRoute(string Method, string Pattern); 
        private readonly List<PublicRoute> _publicRoutes; 

        public GatewayJwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;

            var jwtSection = configuration.GetSection("JwtSettings");
            _issuer = jwtSection["Issuer"]!;
            _audiences = jwtSection.GetSection("Audiences").Get<List<string>>()!;
            _publicRoutes = configuration.
                GetSection("PublicRoutes")
                .Get<List<string>>()!
                .Select(entry =>
                {
                    var parts = entry.Trim().Split(' ', 2);
                    return new PublicRoute(
                        Method: parts[0].ToUpper(),
                        Pattern: parts[1]
                    );
                }).ToList(); 

            var publicKeyPem = jwtSection["RsaPublicKey"]!.Replace("\\n", "\n");
            var rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyPem);
            _rsaPublicKey = new RsaSecurityKey(rsa); 
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? "";
            var method = context.Request.Method;

            if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            if (path.StartsWith("/health", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return; 
            }

            // if (path.StartsWith("", StringComparison.OrdinalIgnoreCase))
            // {
            //     await _next(context);
            //     return; 
            // }

            if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/health", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/ui/", StringComparison.OrdinalIgnoreCase) ||        
                path.Equals("/favicon.ico", StringComparison.OrdinalIgnoreCase))       
            {
                await _next(context);
                return;
            }


            if (IsPublicRoute(method,path))
            {
                await _next(context);
                return;
            }

            var token = ExtractBearerToken(context);

            if (string.IsNullOrEmpty(token))
            {
                await WriteResponse(context, 401, "Token no proporcionado");
                return;
            }

            if (!TryValidateToken(token, out var principal))
            {
                await WriteResponse(context, 401, "Token inválido o expirado");
                return;
            }

            var userId = principal?.FindFirst("userId")?.Value;
            var email = principal?.FindFirst("email")?.Value
                      ?? principal?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            if (!string.IsNullOrEmpty(userId))
                context.Request.Headers["X-User-Id"] = userId;

            if (!string.IsNullOrEmpty(email))
                context.Request.Headers["X-User-Email"] = email;

            await _next(context);
        }

        private bool IsPublicRoute(string method, string rawPath)
        {
            var path = rawPath.Split('?')[0]; // ignora query params

            return _publicRoutes.Any(route => 
                route.Method.Equals(method, StringComparison.OrdinalIgnoreCase) && 
                MatchesPattern(path, route.Pattern));
        }

        private static bool MatchesPattern(string path, string pattern)
        {
            var pathSegments = path.Trim('/').Split('/');
            var patternSegments = pattern.Trim('/').Split('/');

            if (pathSegments.Length != patternSegments.Length)
                return false;

            for (int i = 0; i < patternSegments.Length; i++)
            {
                var patternSegment = patternSegments[i];
                var pathSegment = pathSegments[i];

                if (patternSegment.StartsWith("{") && patternSegment.EndsWith("}"))
                    continue;

                if (!patternSegment.Equals(pathSegment, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        private static string? ExtractBearerToken(HttpContext context)
        {
            var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
            return authHeader?.StartsWith("Bearer") == true
                ? authHeader["Bearer ".Length..].Trim()
                : null; 
        }

        private bool TryValidateToken(string token, out System.Security.Claims.ClaimsPrincipal? principal)
        {
            principal = null;
            try
            {
                var handler = new JsonWebTokenHandler();
                var result = handler.ValidateTokenAsync(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _rsaPublicKey,
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudiences = _audiences,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }).GetAwaiter().GetResult(); // sync over async en constructor-adjacent code

                if (!result.IsValid) return false;

                principal = new System.Security.Claims.ClaimsPrincipal(result.ClaimsIdentity);
                return true;
            }
            catch { return false; }
        }

        private static async Task WriteResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(
                JsonSerializer.Serialize(new { message })); 
        }
    }
}
