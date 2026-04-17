using BuildingBlocks.CQRS;
using Mapster;
using Notification.API.Repositories.NotificationRepository;

namespace Notification.API.Features.Notification.GetNumberOfNotificationsByUser
{
    public class GetNumberOfNotificationsByUserQueryHandler : IQueryHandler<GetNumberOfNotificationsByUserQuery, GetNumberOfNotificationsByUserResponse>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetNumberOfNotificationsByUserQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<GetNumberOfNotificationsByUserResponse> Handle(GetNumberOfNotificationsByUserQuery query, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetNumberOfNotificationsUnReadByUserIdAsync(query.UserId);

            var mapNotifications = notifications.Adapt<List<NotificationResponse>>();

            return new GetNumberOfNotificationsByUserResponse(mapNotifications); 
        }
    }
}
