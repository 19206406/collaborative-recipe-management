using BuildingBlocks.Exceptions;
using System.Security.Claims;

namespace BuildingBlocks.Jwt.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst("userId");
            if (claim is null) throw new UnauthorizedException("userId no encontrado");
            return int.Parse(claim.Value); 
        }

        public static string GetEmail(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(System.Security.Claims.ClaimTypes.Email)
                ?? user.FindFirst("email");
            if (claim is null) throw new UnauthorizedException("Email no encontrado");
            return claim.Value; 
        }

        public static string GetName(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst("name");
            if (claim is null) throw new UnauthorizedException("Nombre no encontrado");
            return claim.Value; 
        }

        public static bool IsAuthenticated(this ClaimsPrincipal user)
        {
            return user.Identity?.IsAuthenticated ?? false; 
        }
    }
}
