namespace BuildingBlocks.Jwt.Models
{
    public class JwtSettings
    {
        // Validación hibrida 

        // generar token 
        public string RsaPrivateKey { get; set; } = string.Empty;

        // verificar token api-gateway y otros servicios 
        public string RsaPublicKey { get; set; } = string.Empty; 

        //public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public List<string> Audiences { get; set; } = new();
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }
}
