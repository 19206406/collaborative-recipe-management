using BuildingBlocks.CQRS;

namespace Notification.API.Features.Notification.CreateAuxiliaryNotification
{
    public record CreateAuxiliaryNotificationCommand(int UserId, string Type, string Title, string Message) 
        : ICommand<CreateAuxiliaryNotificationResponse>; 
}
