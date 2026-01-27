using FastEndpoints;
using MediatR;

namespace User.API.Features.UserPreference.UpdatePreferencesToUser
{
    public record UpdateReferences(int Id, string PreferenceType, DateTime CreatedAt, int UserId);

    public record UpdatePreferencesToUserRequest(int Id, List<UpdateReferences> Preferences);

    public class UpdatePreferencesToUserEndpoint : Endpoint<UpdatePreferencesToUserRequest, UpdatePreferencesToUserResponse>
    {
        private readonly IMediator _mediator;

        public UpdatePreferencesToUserEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("api/users/{id}/preferences");
            Summary(x =>
            {
                x.Summary = "Actualizar preferencias de usuario";
                x.Description = "Actualizar las preferencias de usuario actualiza, agrega y elimina las preferencias especificadas";
            });
            Description(x => x.WithTags("UserPreferences")); 
        }

        public override async Task HandleAsync(UpdatePreferencesToUserRequest req, CancellationToken ct)
        {
            var updatePreferences = req.Preferences.Select(x =>
                new Entities.UserPreference
                {
                    Id = x.Id,
                    PreferenceType = x.PreferenceType,
                    CreatedAt = x.CreatedAt, 
                    UserId = x.UserId
                }
            ).ToList(); 

            var command = new UpdatePreferencesToUserCommand(req.Id, updatePreferences);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result); 
        }
    }
}
