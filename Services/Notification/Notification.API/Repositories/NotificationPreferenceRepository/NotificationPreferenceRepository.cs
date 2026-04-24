using Microsoft.EntityFrameworkCore;
using Notification.API.Common.Database;
using Notification.API.Entities;

namespace Notification.API.Repositories.NotificationPreferenceRepository
{
    public class NotificationPreferenceRepository : INotificationPreferenceRepository
    {
        private readonly NotificationDbContext _context;

        public NotificationPreferenceRepository(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task CreatePreferencesByUserAsync(NotificationPreference userPreferences)
        {
            _context.Add(userPreferences);
            await _context.SaveChangesAsync(); 
        }

        public async Task<NotificationPreference?> GetPreferencesByUserIdAsync(int userId)
        {
            var preferences = await _context.NotificationPreferences
                .FirstOrDefaultAsync(x => x.UserId == userId); 

            return preferences; 
        }

        public async Task UpdatePreferencesByUserIdAsync()
        {
            await _context.SaveChangesAsync(); 
        }
    }
}
