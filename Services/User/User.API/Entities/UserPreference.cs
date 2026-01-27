namespace User.API.Entities
{
    public class UserPreference 
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PreferenceType { get; set; } = default!;
        public DateTime CreatedAt { get; set; }

        public User User { get; set; } = new();
    }
}

