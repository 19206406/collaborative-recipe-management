using BuildingBlocks.Jwt.Claims;
using FastEndpoints;
using MediatR;

namespace Notification.API.Features.NotificationPreference.UpdatePreferencesByUser
{
    public record NotificationPreference(int Id, byte EmailNotifications, byte PushNotifications); 
    public record UpdatePreferencesByUserRequest(List<NotificationPreference> NotificationPreferences); 

    public class UpdatePreferencesByUserEndpoint : Endpoint<UpdatePreferencesByUserRequest, UpdatePreferencesByUserResponse>
    {
        private readonly IMediator _mediator;

        public UpdatePreferencesByUserEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("/api/notifications/preferences");
            Summary(x =>
            {
                x.Summary = "Actualizar preferencias usuario";
                x.Description = "Actualizar las preferencias de un usuario";
            });

            Description(x => x.WithTags("NotificationPreferences")); 
        }

        public override async Task HandleAsync(UpdatePreferencesByUserRequest req, CancellationToken ct)
        {
            var userId = HttpContext.User.GetUserId();
            var command = new UpdatePreferencesByUserCommand(userId, req.NotificationPreferences);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result); 
        }
    }
}
