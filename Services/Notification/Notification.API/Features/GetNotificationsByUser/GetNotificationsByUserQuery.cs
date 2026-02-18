using BuildingBlocks.CQRS;

namespace Notification.API.Features.GetNotificationsByUser
{
    public record GetNotificationsByUserQuery(int UserId) : IQuery<GetNotificationsByUserResponse>; 
}
