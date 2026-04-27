using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text.Json;

namespace ApiGateway.Middleware
{
	public class GatewayJwtMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly RsaSecurityKey _rsaPublicKey;
		private readonly string _issuer;
		private readonly List<string> _audiences;
		private readonly List<string> _publicRoutes;

		public GatewayJwtMiddleware(RequestDelegate next, IConfiguration configuration)
		{
			_next = next;

			var jwtSection = configuration.GetSection("JwtSettings");
			_issuer = jwtSection["Issuer"]!;
			_audiences = jwtSection.GetSection("Audiences").Get<List<string>>()!;
			_publicRoutes = configuration.GetSection("PublicRoutes").Get<List<string>>() ?? new();

			// El Gateway solo necesita la clave PÚBLICA para verificar
			var publicKeyPem = jwtSection["RsaPublicKey"]!.Replace("\\n", "\n");
			var rsa = RSA.Create();
			rsa.ImportFromPem(publicKeyPem);
			_rsaPublicKey = new RsaSecurityKey(rsa);
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var path = context.Request.Path.Value ?? "";

			// Rutas públicas (login, registro) pasan sin validar token
			if (_publicRoutes.Any(r => path.StartsWith(r, StringComparison.OrdinalIgnoreCase)))
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

			// Reenvía información del usuario como headers para los microservicios
			// Así evitan tener que re-parsear el token solo para obtener el userId
			var userId = principal?.FindFirst("userId")?.Value;
			var email = principal?.FindFirst("email")?.Value
					  ?? principal?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

			if (!string.IsNullOrEmpty(userId))
				context.Request.Headers["X-User-Id"] = userId;

			if (!string.IsNullOrEmpty(email))
				context.Request.Headers["X-User-Email"] = email;

			// ✅ Token válido — YARP reenvía la request al microservicio
			// El microservicio hará su propia validación (segunda capa)
			await _next(context);
		}

		private static string? ExtractBearerToken(HttpContext context)
		{
			var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
			return authHeader?.StartsWith("Bearer ") == true
				? authHeader["Bearer ".Length..]
				: null;
		}

		private bool TryValidateToken(string token, out System.Security.Claims.ClaimsPrincipal? principal)
		{
			principal = null;
			try
			{
				principal = new JwtSecurityTokenHandler().ValidateToken(
					token,
					new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = _rsaPublicKey,
						ValidateIssuer = true,
						ValidIssuer = _issuer,
						ValidateAudience = true,
						ValidAudiences = _audiences,
						ValidateLifetime = true,
						ClockSkew = TimeSpan.Zero
					},
					out _);
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