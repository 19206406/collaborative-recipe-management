using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Mapster;
using Notification.API.Repositories.NotificationPreferenceRepository;

namespace Notification.API.Features.NotificationPreference.UpdatePreferencesByUser
{
    public class UpdatePreferencesByUserCommandHandler : ICommandHandler<UpdatePreferencesByUserCommand, UpdatePreferencesByUserResponse>
    {
        private readonly INotificationPreferenceRepository _notificationPreferenceRepository;

        public UpdatePreferencesByUserCommandHandler(INotificationPreferenceRepository notificationPreferenceRepository)
        {
            _notificationPreferenceRepository = notificationPreferenceRepository;
        }

        public async Task<UpdatePreferencesByUserResponse> Handle(UpdatePreferencesByUserCommand command, CancellationToken cancellationToken)
        {
            var notificationPreferences = await _notificationPreferenceRepository.GetPreferencesByUserIdAsync(command.UserId);

            if (notificationPreferences is null)
                throw new NotFoundException("preferencias de notificacación de usuario", command.Id); 

            notificationPreferences.PushNotifications = command.PushNotifications;
            notificationPreferences.EmailNotifications = command.EmailNotifications; 
            
            await _notificationPreferenceRepository.UpdatePreferencesByUserIdAsync();

            return notificationPreferences.Adapt<UpdatePreferencesByUserResponse>(); 
        }
    }
}
