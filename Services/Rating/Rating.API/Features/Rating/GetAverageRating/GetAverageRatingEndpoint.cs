using FastEndpoints;
using MediatR;

namespace Rating.API.Features.Rating.GetAverageRating
{
    public record GetAverageRatingRequest(int RecipeId); 
    public class GetAverageRatingEndpoint : Endpoint<GetAverageRatingRequest, GetAverageRatingResponse>
    {
        private readonly IMediator _mediator;

        public GetAverageRatingEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("api/ratings/recipe/{RecipeId}/average");
            Summary(x =>
            {
                x.Summary = "Obtiene el promedio de calificaciones para una receta específica.";
                x.Description = "Devuelve el promedio de calificaciones para la receta identificada por RecipeId.";
            });

            Description(x => x.WithTags("Ratings"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(GetAverageRatingRequest req, CancellationToken ct)
        {
            var query = new GetAverageRatingQuery(req.RecipeId);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
