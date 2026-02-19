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

        public async Task<Entities.Notification?> GetNotificationByIdAsync(int id)
        {
            var notification = await _context.Notifications
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return notification; 
        }

        public async Task<IEnumerable<Entities.Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return notifications; 
        }

        public async Task<bool> UpdateAllNotificationsAsync(IEnumerable<Entities.Notification> notifications)
        {
            _context.Notifications.UpdateRange(notifications);
            return await _context.SaveChangesAsync() > 0; 
        }

        public async Task<Entities.Notification> UpdateNotificationAsync(Entities.Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();

            await _context.Entry(notification).ReloadAsync();
            return notification; 
        }
    }
}
