using BuildingBlocks.CQRS;

namespace Notification.API.Features.Notification.MarkAsRead
{
    public record MarkAsReadCommand(int Id, int UserId, bool IsRead) : ICommand<MarkAsReadResponse>; 
}
