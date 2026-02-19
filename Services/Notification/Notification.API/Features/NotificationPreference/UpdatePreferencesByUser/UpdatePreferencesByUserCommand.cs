using BuildingBlocks.CQRS;

namespace Notification.API.Features.NotificationPreference.UpdatePreferencesByUser
{
    public record UpdatePreferencesByUserCommand(
        int UserId, 
        List<NotificationPreference> NotificationPreferences) 
        : ICommand<UpdatePreferencesByUserResponse>; 
}
