using BuildingBlocks.Jwt.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BuildingBlocks.Jwt.Service
{
    public static class JwtValidationExtensions
    {
        public static IServiceCollection AddJwtValidation(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings(); 
            configuration.GetSection("JwtSettings").Bind(jwtSettings);

            var rsa = RsaKeyHelper.LoadPublicKey(jwtSettings.RsaPublicKey);
            var rsaSecurityKey = new RsaSecurityKey(rsa); 

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudiences = jwtSettings.Audiences,
                        IssuerSigningKey = rsaSecurityKey, 
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = JwtEvents.CustomJwtEvents();
                });

            // Agregamos Authorization para que funcione con el middleware estándar
            services.AddAuthorization();

            return services;
        }
    }
}