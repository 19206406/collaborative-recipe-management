using BuildingBlocks.CQRS;
using Mapster;
using Notification.API.Repositories.NotificationPreferenceRepository;

namespace Notification.API.Features.NotificationPreference.GetPreferencesByUser
{
    public class GetPreferencesByUserQueryHandler : IQueryHandler<GetPreferencesByUserQuery, GetPreferencesByUserResponse>
    {
        private readonly INotificationPreferenceRepository _notificationPreferenceRepository;

        public GetPreferencesByUserQueryHandler(INotificationPreferenceRepository notificationPreferenceRepository)
        {
            _notificationPreferenceRepository = notificationPreferenceRepository;
        }

        public async Task<GetPreferencesByUserResponse> Handle(GetPreferencesByUserQuery query, CancellationToken cancellationToken)
        {
            var notificationPreferences = await _notificationPreferenceRepository.GetPreferencesByUserIdAsync(query.UserId);

            if (notificationPreferences is null)
            {
                var userPreferences = new Entities.NotificationPreference
                {
                    UserId = query.UserId,
                    EmailNotifications = 1,
                    PushNotifications = 1,
                };

                await _notificationPreferenceRepository.CreatePreferencesByUserAsync(userPreferences);

                return userPreferences.Adapt<GetPreferencesByUserResponse>(); 
            }

            return notificationPreferences.Adapt<GetPreferencesByUserResponse>(); 
        }
    }
}
