using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace BuildingBlocks.Jwt.Service
{
    public static class JwtEvents
    {
        public static JwtBearerEvents CustomJwtEvents()
        {
            return new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception is SecurityTokenExpiredException)
                        context.Response.Headers.Add("Token-Expired", "true");
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var result = JsonSerializer.Serialize(new
                    {
                        message = "No está autenticado o el token es inválido"
                    });

                    return context.Response.WriteAsync(result);
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";

                    var result = JsonSerializer.Serialize(new
                    {
                        message = "No tiene permisos para acceder a este recurso"
                    });

                    return context.Response.WriteAsync(result);
                }

            };
        }
    }
}
