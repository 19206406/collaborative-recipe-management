using BuildingBlocks.CQRS;
using Mapster;
using Notification.API.Repositories.NotificationRepository;

namespace Notification.API.Features.Notification.CreateAuxiliaryNotification
{
    public class CreateAuxiliaryNotificationCommandHandler : ICommandHandler<CreateAuxiliaryNotificationCommand, CreateAuxiliaryNotificationResponse>
    {
        private readonly INotificationRepository _notificationRepository;

        public CreateAuxiliaryNotificationCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<CreateAuxiliaryNotificationResponse> Handle(CreateAuxiliaryNotificationCommand command, CancellationToken cancellationToken)
        {
            var notification = new Entities.Notification
            {
                UserId = command.UserId,
                Type = command.Type,
                Title = command.Title,
                Message = command.Message,
                IsRead = 0,
                CreatedAt = DateTime.UtcNow
            };

            var newNotification = await _notificationRepository.AddNotificationAsync(notification);

            return newNotification.Adapt<CreateAuxiliaryNotificationResponse>(); 
        }
    }
}
