using BuildingBlocks.CQRS;

namespace Notification.API.Features.NotificationPreference.UpdatePreferencesByUser
{
    public record UpdatePreferencesByUserCommand(
        int UserId, int Id, byte EmailNotifications, byte PushNotifications) 
        : ICommand<UpdatePreferencesByUserResponse>; 
}
