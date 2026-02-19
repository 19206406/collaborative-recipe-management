using BuildingBlocks.CQRS;

namespace Notification.API.Features.Notification.MarkAsRead
{
    public record MarkAsReadCommand(int Id, bool Read) : ICommand<MarkAsReadResponse>; 
}
