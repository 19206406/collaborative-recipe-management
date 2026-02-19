using BuildingBlocks.Jwt.Claims;
using FastEndpoints;
using MediatR;

namespace Notification.API.Features.Notification.MarkAllNotificationsAsRead
{
    public class MarkAllNotificationsAsReadEndpoint : EndpointWithoutRequest<MarkAllNotificationsAsReadResponse>
    {
        private readonly IMediator _mediator;

        public MarkAllNotificationsAsReadEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("/api/notifications/read-all");
            Summary(x =>
            {
                x.Summary = "Marcar todas las notificaciones como leidas.";
                x.Description = "Marcar todas las notificaciones como leidas.";
            });
            Description(x => x.WithTags("Notifications")); 
        }

        public async override Task HandleAsync(CancellationToken ct)
        {
            var userId = HttpContext.User.GetUserId();

            var command = new MarkAllNotificationsAsReadCommand(userId);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result);
        }
    }
}
