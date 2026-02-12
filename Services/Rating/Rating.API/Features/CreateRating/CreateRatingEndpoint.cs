using FastEndpoints;
using MediatR;

namespace Rating.API.Features.CreateRating
{
    public record CreateRatingRequest(int UserId, int RecipeId, int Rating, string? Comment); 

    public class CreateRatingEndpoint : Endpoint<CreateRatingRequest, CreateRatingResponse>
    {
        private readonly IMediator _mediator;

        public CreateRatingEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("api/ratings/only");
            Summary(x =>
            {
                x.Summary = "Crear y actualizar una recomendación";
                x.Description = "Crea una nueva recomendación para una receta específica. Si el usuario ya ha calificado la receta, se actualizará la calificación existente.";
            });
            Description(x => x.WithTags("Ratings"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(CreateRatingRequest req, CancellationToken ct)
        {
            var command = new CreateRatingCommand(req.UserId, req.RecipeId, req.Rating, req.Comment);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result); 
        }
    }
}
