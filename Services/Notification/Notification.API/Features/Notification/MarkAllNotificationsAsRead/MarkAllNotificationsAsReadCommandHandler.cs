using BuildingBlocks.CQRS;
using Mapster;
using Notification.API.Features.Notification.GetNotificationsByUser;
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

            // validar que exista usuario 

            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(command.UserId); 

            foreach (var notification in notifications)
            {
                notification.IsRead = 1; 
            }

            await _notificationRepository.UpdateAllNotificationsAsync(notifications); 

            return new MarkAllNotificationsAsReadResponse(notifications.Adapt<List<NotificationResponse>>()); 
        }
    }
}
