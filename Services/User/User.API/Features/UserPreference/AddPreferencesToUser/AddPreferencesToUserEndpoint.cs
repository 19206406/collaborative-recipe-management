using FastEndpoints;
using MediatR;

namespace User.API.Features.UserPreference.AddPreferencesToUser
{
    public record AddPreferences(string PreferenceType); 

    public record AddPreferencesToUserRequest(int Id, List<AddPreferences> Preferences); 

    public class AddPreferencesToUserEndpoint : Endpoint<AddPreferencesToUserRequest, AddPreferencesToUserResponse>
    {
        private readonly IMediator _mediator;

        public AddPreferencesToUserEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("api/users/{id}/preferences");
            Summary(x =>
            {
                x.Summary = "Agregar preferencias";
                x.Description = "Agrega preferencias a un usuario todos las preferencias " +
                "son nuevas para dicho usuario esta acción no permite actualizar";
            }); 
            Description(x => x.WithTags("UserPreferences")); 
        }

        public override async Task HandleAsync(AddPreferencesToUserRequest req, CancellationToken ct)
        {
            var newPreferences = req.Preferences.Select(x => new Entities.UserPreference
            {
                PreferenceType = x.PreferenceType,
            }).ToList(); 

            var command = new AddPreferencesToUserCommand(req.Id, newPreferences);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result); 
        }
    }
}
