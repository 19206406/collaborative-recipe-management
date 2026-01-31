using FastEndpoints;
using MediatR;

namespace User.API.Features.UserPreference.GetUserPreferences
{
    public record GetUserPreferencesRequest(int Id); 

    public class GetUserPreferencesEndpoint : Endpoint<GetUserPreferencesRequest, GetUserPreferencesResponse>
    {
        private readonly IMediator _mediator;

        public GetUserPreferencesEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public const string Route = "api/users/{id}/preferences"; 

        public override void Configure()
        {
            Get(Route);
            Summary(x =>
            {
                x.Summary = "Obtener un usuario con sus preferencias";
                x.Description = "Obtener un usuario con sus preferencias";
            });
            Description(x => x.WithTags("UserPreferences"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(GetUserPreferencesRequest req, CancellationToken ct)
        {
            var query = new GetUserPreferencesQuery(req.Id);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
