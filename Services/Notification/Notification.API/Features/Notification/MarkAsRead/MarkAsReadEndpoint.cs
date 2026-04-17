using BuildingBlocks.Jwt.Claims;
using FastEndpoints;
using MediatR;

namespace Notification.API.Features.Notification.MarkAsRead
{
    public record MarkAsReadRequest(int Id, bool Read); 

    public class MarkAsReadEndpoint : Endpoint<MarkAsReadRequest, MarkAsReadResponse>
    {
        private readonly IMediator _mediator;

        public MarkAsReadEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("/api/notifications/{id}/read");
            Summary(x =>
            {
                x.Summary = "Marcar una notificación como leída";
                x.Description = "Marcar una notificación como leída";
            });
            Description(x => x.WithTags("Notifications")); 
        }

        public override async Task HandleAsync(MarkAsReadRequest req, CancellationToken ct)
        {
            var userId = HttpContext.User.GetUserId(); 

            var command = new MarkAsReadCommand(req.Id, userId, req.Read);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result); 
        }
    }
}
