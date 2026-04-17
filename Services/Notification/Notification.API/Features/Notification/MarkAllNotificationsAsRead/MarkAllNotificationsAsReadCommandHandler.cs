using BuildingBlocks.CQRS;
using Mapster;
using Notification.API.Repositories.NotificationRepository;

namespace Notification.API.Features.Notification.MarkAllNotificationsAsRead
{
    public class MarkAllNotificationsAsReadCommandHandler 
        : ICommandHandler<MarkAllNotificationsAsReadCommand, MarkAllNotificationsAsReadResponse>
    {
        private readonly INotificationRepository _notificationRepository;

        public MarkAllNotificationsAsReadCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<MarkAllNotificationsAsReadResponse> Handle(MarkAllNotificationsAsReadCommand command, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetNumberOfNotificationsUnReadByUserIdAsync(command.UserId); 

            foreach (var notification in notifications)
            {
                notification.IsRead = 1; 
            }

            await _notificationRepository.UpdateNotificationsAsync();
            var mapNotification = notifications.Adapt<List<NotificationResponse>>(); 

            return new MarkAllNotificationsAsReadResponse(mapNotification); 
        }
    }
}
