using BuildingBlocks.Jwt.Claims;
using FastEndpoints;
using MediatR;

namespace Notification.API.Features.NotificationPreference.GetPreferencesByUser
{
    public class GetPreferencesByUserEndpoint : EndpointWithoutRequest<GetPreferencesByUserResponse>
    {
        private readonly IMediator _mediator;

        public GetPreferencesByUserEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/api/notifications/preferences");
            Summary(x =>
            {
                x.Summary = "Get notification preferences";
                x.Description = "Returns the notification preferences of the currently authenticated user.";
            });
            Description(x => x.WithTags("NotificationPreferences"));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userId = HttpContext.User.GetUserId();
            var query = new GetPreferencesByUserQuery(userId);

            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
