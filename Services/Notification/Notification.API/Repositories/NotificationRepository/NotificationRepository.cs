using Microsoft.EntityFrameworkCore;
using Notification.API.Common.Database;

namespace Notification.API.Repositories.NotificationRepository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDbContext _context;

        public NotificationRepository(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Entities.Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            var notifications = await _context.Notifications
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return notifications; 
        }
    }
}
