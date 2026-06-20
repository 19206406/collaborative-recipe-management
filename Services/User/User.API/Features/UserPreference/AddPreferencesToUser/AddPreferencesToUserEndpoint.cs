using FastEndpoints;
using MediatR;
using User.API.Features.UserPreference.GetUserPreferences;

namespace User.API.Features.UserPreference.AddPreferencesToUser
{
    public record AddPreferences(string preference); 

    public record AddPreferencesToUserRequest(int Id, List<string> Preferences); 

    public class AddPreferencesToUserEndpoint : Endpoint<AddPreferencesToUserRequest, AddPreferencesToUserResponse>
    {
        private readonly IMediator _mediator;

        public AddPreferencesToUserEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/api/users/{id}/preferences");
            Summary(x =>
            {
                x.Summary = "Add user preferences";
                x.Description = "Adds new preferences to a user. All preferences provided must be new for the given user. This action does not update existing preferences.";
            });
            Description(x => x.WithTags("UserPreferences")); 
        }

        public override async Task HandleAsync(AddPreferencesToUserRequest req, CancellationToken ct)
        {
            var command = new AddPreferencesToUserCommand(req.Id, req.Preferences);
            var result = await _mediator.Send(command);

            await Send.CreatedAtAsync(GetUserPreferencesEndpoint.Route, new { id = result.Id }, result); 
        }
    }
}
