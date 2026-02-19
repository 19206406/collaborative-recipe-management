using BuildingBlocks.CQRS;
using Mapster;
using Notification.API.Features.NotificationPreference.GetPreferencesByUser;
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
            var userPreferences = await _notificationPreferenceRepository.GetPreferencesByUserIdAsync(command.UserId); 

            foreach (var preferenceNot in command.NotificationPreferences)
            {
                var preference = userPreferences.First(x => x.Id == preferenceNot.Id); 

                preference.PushNotifications = preferenceNot.PushNotifications;
                preference.EmailNotifications = preferenceNot.EmailNotifications; 
            }

            await _notificationPreferenceRepository.UpdatePreferencesByUserIdAsync(userPreferences.ToList());

            return new UpdatePreferencesByUserResponse(userPreferences.Adapt<List<PreferencesResponse>>()); 
        }
    }
}
