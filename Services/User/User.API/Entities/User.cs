namespace User.API.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public byte IsActive { get; set; }

        public ICollection<UserPreference> UserPreferences { get; set; } = new List<UserPreference>(); 
    }
}

