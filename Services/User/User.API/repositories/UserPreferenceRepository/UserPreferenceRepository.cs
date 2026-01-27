using Microsoft.EntityFrameworkCore;
using User.API.Common.Database;
using User.API.Entities;

namespace User.API.repositories.UserPreferenceRepository
{
    public class UserPreferenceRepository : IUserPreferenceRespository
    {
        private readonly UserDbContext _context;

        public UserPreferenceRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserPreference>> GetPreferences()
        {
            var preferences = await _context.UserPreferences
                .AsNoTracking().ToListAsync();
            return preferences; 
        }

        public async Task<Entities.User> GetUserPreferences(int id)
        {
            var userWithPreferences = await _context.Users
                .Include(u => u.UserPreferences)
                .FirstOrDefaultAsync(u => u.Id == id);

            return userWithPreferences; 
        }

        public async Task<bool> AddUserPreferences(int userId, List<UserPreference> items)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is null) return false; 

            foreach (var item in items)
            {
                item.UserId = userId;
                item.CreatedAt = DateTime.UtcNow; 
            }

            _context.UserPreferences.AddRange(items);
            return await _context.SaveChangesAsync() > 0; 
        }

        public async Task<bool> RemoveReferences(List<UserPreference> items)
        {
            _context.UserPreferences.RemoveRange(items);
            return await _context.SaveChangesAsync() > 0; 
        }

        public async Task<bool> UpdateReferences(List<UserPreference> items)
        {
            _context.UserPreferences.UpdateRange(items);
            return await _context.SaveChangesAsync() > 0; 
        }
    }
}
