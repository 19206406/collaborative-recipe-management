using BuildingBlocks.CQRS;

namespace Notification.API.Features.NotificationPreference.GetPreferencesByUser
{
    public record GetPreferencesByUserQuery(int UserId) : IQuery<GetPreferencesByUserResponse>; 
}
