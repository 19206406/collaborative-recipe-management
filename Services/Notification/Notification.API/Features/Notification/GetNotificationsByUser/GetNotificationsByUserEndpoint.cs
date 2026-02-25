using BuildingBlocks.Jwt.Claims;
using FastEndpoints;
using MediatR;

namespace Notification.API.Features.Notification.GetNotificationsByUser
{

    public class GetNotificationsByUserEndpoint : EndpointWithoutRequest<GetNotificationsByUserResponse>
    {
        private readonly IMediator _mediator;

        public GetNotificationsByUserEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/api/notifications");
            Summary(x =>
            {
                x.Summary = "Obtener las notificaciones de un usuario";
                x.Description = "Obtener las notificaciones de un usuario por medio de su id";
            }); 
            Description(x => x.WithTags("Notifications")); 
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userId = HttpContext.User.GetUserId();

            var query = new GetNotificationsByUserQuery(userId);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
