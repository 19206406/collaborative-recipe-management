using FastEndpoints;
using MediatR;

namespace Rating.API.Features.Rating.CreateAndUpdateRating
{
    public record CreateAndUpdateRatingRequest(int Id, int UserId, int RecipeId, int Rating, string? Comment, bool IsToUpdate); 
    public class CreateAndUpdateRatingEndpoint : Endpoint<CreateAndUpdateRatingRequest, CreateAndUpdateRatingResponse>
    {
        private readonly IMediator _mediator;

        public CreateAndUpdateRatingEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("api/ratings");
            Summary(x =>
            {
                x.Summary = "Crear o actualizar una calificación";
                x.Description = "Permite crear o actualizar una calificación para una receta específica.";
            });
            Description(x => x.WithTags("Ratings"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(CreateAndUpdateRatingRequest req, CancellationToken ct)
        {
            var command = new CreateAndUpdateRatingCommand(req.Id, req.UserId, req.RecipeId, req.Rating, req.Comment, req.IsToUpdate);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result); 
        }
    }
}
