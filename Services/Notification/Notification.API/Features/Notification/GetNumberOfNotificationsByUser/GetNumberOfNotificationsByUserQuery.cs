using BuildingBlocks.CQRS;

namespace Notification.API.Features.Notification.GetNumberOfNotificationsByUser
{
    public record GetNumberOfNotificationsByUserQuery(int UserId) : IQuery<GetNumberOfNotificationsByUserResponse>; 
}
