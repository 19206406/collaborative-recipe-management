using BuildingBlocks.CQRS;
using Notification.API.Repositories.NotificationRepository;

namespace Notification.API.Features.Notification.GetNotificationsByUser
{
    public class GetNotificationsByUserQueryHandler : IQueryHandler<GetNotificationsByUserQuery, GetNotificationsByUserResponse>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetNotificationsByUserQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<GetNotificationsByUserResponse> Handle(GetNotificationsByUserQuery query, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(query.UserId);
            var mapNotifications = notifications.

            return new GetNotificationsByUserResponse(); 
        }
    }
}
