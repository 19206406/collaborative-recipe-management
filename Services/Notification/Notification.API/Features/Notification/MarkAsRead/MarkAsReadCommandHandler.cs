using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Notification.API.Repositories.NotificationRepository;

namespace Notification.API.Features.Notification.MarkAsRead
{
    public class MarkAsReadCommandHandler : ICommandHandler<MarkAsReadCommand, MarkAsReadResponse>
    {
        private readonly INotificationRepository _notificationRepository;

        public MarkAsReadCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<MarkAsReadResponse> Handle(MarkAsReadCommand command, CancellationToken cancellationToken)
        {
            var notif = await _notificationRepository.GetNotificationByIdAsync(command.Id);

            if (notif is null)
                throw new NotFoundException("notificación", command.Id);

            notif.IsRead = Convert.ToByte(command.Read);

            var updatedNotification = await _notificationRepository.UpdateNotificationAsync(notif); 

            return new MarkAsReadResponse(
                updatedNotification.Id, updatedNotification.UserId, updatedNotification.Type, 
                updatedNotification.Title, updatedNotification.Message, updatedNotification.IsRead, 
                updatedNotification.CreatedAt); 
        }
    }
}
