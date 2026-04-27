using System.Security.Cryptography;

namespace BuildingBlocks.Jwt.Service
{
    public static class RsaKeyHelper
    {
        public static RSA LoadPublicKey(string publicKeyPem)
        {
            if (string.IsNullOrWhiteSpace(publicKeyPem)) 
                throw new InvalidOperationException("RsaPublicKey no está configurado."); 

            var rsa = RSA.Create(); 
            rsa.ImportFromPem(NormalizeKey(publicKeyPem)); 
            return rsa; 
        }

        public static RSA LoadPrivateKey(string privateKeyPem)
        {
            if (string.IsNullOrWhiteSpace(privateKeyPem))
                throw new InvalidOperationException("RsaPrivateKey no está configurado.");

            var rsa = RSA.Create();
            rsa.ImportFromPem(NormalizeKey(privateKeyPem));
            return rsa; 
        }

        private static string NormalizeKey(string key) => key.Replace("\\n", "\n"); 
    }
}
