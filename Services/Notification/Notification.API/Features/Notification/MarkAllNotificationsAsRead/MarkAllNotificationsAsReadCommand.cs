using BuildingBlocks.CQRS;

namespace Notification.API.Features.Notification.MarkAllNotificationsAsRead
{
    public record MarkAllNotificationsAsReadCommand(int UserId) : ICommand<MarkAllNotificationsAsReadResponse>; 
}
