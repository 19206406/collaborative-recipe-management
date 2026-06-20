using BuildingBlocks.Jwt.Claims;
using FastEndpoints;
using MediatR;

namespace Notification.API.Features.Notification.GetNumberOfNotificationsByUser
{
    public class GetNumberOfNotificationsByUserEndpoint : EndpointWithoutRequest<List<GetNumberOfNotificationsByUserResponse>>
    {
        private readonly IMediator _mediator;

        public GetNumberOfNotificationsByUserEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/api/notifications/without-reading");
            Summary(x =>
            {
                x.Summary = "Get unread notifications";
                x.Description = "Returns all unread notifications for the currently authenticated user.";
            });
            Description(x => x.WithTags("Notifications"));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userId = HttpContext.User.GetUserId();
            var query = new GetNumberOfNotificationsByUserQuery(userId);

            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
