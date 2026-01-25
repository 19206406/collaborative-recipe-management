namespace User.API.PasswordHash
{
    public interface IPasswordHashService
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword); 
    }
}
