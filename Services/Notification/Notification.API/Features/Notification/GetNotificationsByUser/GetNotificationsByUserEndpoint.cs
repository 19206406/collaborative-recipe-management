using BuildingBlocks.Jwt.Claims;
using FastEndpoints;
using MediatR;

namespace Notification.API.Features.Notification.GetNotificationsByUser
{

    public class GetNotificationsByUserEndpoint : EndpointWithoutRequest<List<GetNotificationsByUserResponse>>
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
                x.Summary = "Get user notifications";
                x.Description = "Returns all notifications for the currently authenticated user.";
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
