using BuildingBlocks.CQRS;

namespace Notification.API.Features.Notification.GetNotificationsByUser
{
    public record GetNotificationsByUserQuery(int UserId) : IQuery<GetNotificationsByUserResponse>; 
}
