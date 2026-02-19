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
            // 
            var notifications = await _notificationPreferenceRepository.GetPreferencesByUserIdAsync(query.UserId);

            return new GetPreferencesByUserResponse(notifications.Adapt<List<PreferencesResponse>>()); 
        }
    }
}
