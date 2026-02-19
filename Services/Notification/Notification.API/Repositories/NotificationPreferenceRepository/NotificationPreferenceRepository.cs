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

        public async Task<IEnumerable<NotificationPreference>> GetPreferencesByUserIdAsync(int userId)
        {
            var preferences = await _context.NotificationPreferences
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return preferences; 
        }

        public async Task<bool> UpdatePreferencesByUserIdAsync(List<NotificationPreference> notificationPreferences)
        {
            _context.NotificationPreferences.UpdateRange(notificationPreferences);
            return await _context.SaveChangesAsync() > 0; 
        }
    }
}
