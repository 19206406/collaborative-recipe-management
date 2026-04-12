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
                .AsNoTracking()
                .Include(u => u.UserPreferences)
                .FirstOrDefaultAsync(u => u.Id == id);

            return userWithPreferences; 
        }

        public async Task<bool> AddUserPreferences(int userId, List<string> items)
        {
            var entities = items.Select(x => new UserPreference
            {
                UserId = userId,
                PreferenceType = x.ToLower(),
                CreatedAt = DateTime.UtcNow
            }); 

            _context.UserPreferences.AddRange(entities);

            await _context.SaveChangesAsync(); 
            return true; 
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
