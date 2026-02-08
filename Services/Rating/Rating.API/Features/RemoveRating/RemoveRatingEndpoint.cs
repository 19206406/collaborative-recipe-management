using FastEndpoints;
using MediatR;

namespace Rating.API.Features.RemoveRating
{
    public record RemoveRatingRequest(int Id); 

    public class RemoveRatingEndpoint : Endpoint<RemoveRatingRequest>
    {
        private readonly IMediator _mediator;

        public RemoveRatingEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Delete("api/ratings/{id}");
            Summary(x =>
            {
                x.Summary = "Elimina una calificación existente.";
                x.Description = "Elimina una calificación existente. Requiere autenticación.";
            });
            Description(x => x.WithTags("Ratings")); 
        }

        public override async Task HandleAsync(RemoveRatingRequest req, CancellationToken ct)
        {
            // verificar autenticación y autorización para realizar esta acción 

            var command = new RemoveRatingCommand(req.Id);
            await _mediator.Send(command);

            await Send.NoContentAsync(); 
        }
    }
}
