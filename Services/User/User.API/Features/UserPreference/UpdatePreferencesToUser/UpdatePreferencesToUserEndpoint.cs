using FastEndpoints;
using MediatR;

namespace User.API.Features.UserPreference.UpdatePreferencesToUser
{

    public record UpdatePreferencesToUserRequest(int Id, List<UpdatePreferences> Preferences);

    public class UpdatePreferencesToUserEndpoint : Endpoint<UpdatePreferencesToUserRequest, UpdatePreferencesToUserResponse>
    {
        private readonly IMediator _mediator;

        public UpdatePreferencesToUserEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("/api/users/{id}/preferences");
            Summary(x =>
            {
                x.Summary = "Update user preferences";
                x.Description = "Updates the user's preferences. This action adds, updates, and removes preferences based on the provided list.";
            });
            Description(x => x.WithTags("UserPreferences")); 
        }

        public override async Task HandleAsync(UpdatePreferencesToUserRequest req, CancellationToken ct)
        {
            var command = new UpdatePreferencesToUserCommand(req.Id, req.Preferences);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result); 
        }
    }
}
