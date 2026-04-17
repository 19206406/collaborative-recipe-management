using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Notification.API.Repositories.NotificationRepository;
using InvalidOperationException = BuildingBlocks.Exceptions.InvalidOperationException;

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
            var notification = await _notificationRepository.GetNotificationByIdAsync(command.Id);

            if (notification is null)
                throw new NotFoundException("notificación", command.Id);

            if (notification.UserId != command.UserId)
                throw new InvalidOperationException("No tienen permiso de ejecutar la acción para este recurso."); 

            notification.IsRead = Convert.ToByte(command.IsRead);

            await _notificationRepository.UpdateNotificationsAsync(); 

            return new MarkAsReadResponse(notification.Id, notification.UserId, notification.Type, notification.Title, 
                notification.Message, notification.IsRead, notification.CreatedAt); 
        }
    }
}
