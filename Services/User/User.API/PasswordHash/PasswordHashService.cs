using Microsoft.AspNetCore.Identity;

namespace User.API.PasswordHash
{
    public class PasswordHashService : IPasswordHashService
    {
        private readonly IPasswordHasher<object> _passwordHasher;

        public PasswordHashService(IPasswordHasher<Object> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password); 
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);

            return result == PasswordVerificationResult.Success ||
                   result == PasswordVerificationResult.SuccessRehashNeeded; 
        }
    }
}
