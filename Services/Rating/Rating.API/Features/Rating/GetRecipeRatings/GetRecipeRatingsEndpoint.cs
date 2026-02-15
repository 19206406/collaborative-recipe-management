using FastEndpoints;
using MediatR;

namespace Rating.API.Features.Rating.GetRecipeRatings
{
    public record GetRecipeRatingsRequest(int RecipeId); 
    public class GetRecipeRatingsEndpoint : Endpoint<GetRecipeRatingsRequest, GetRecipeRatingsResponse>
    {
        private readonly IMediator _mediator;

        public GetRecipeRatingsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/api/ratings/recipe/{recipeId}");
            Summary(x =>
            {
                x.Summary = "Obtiene las calificaciones de una receta específica.";
                x.Description = "Devuelve una lista de calificaciones para la receta indicada por su ID.";
            });
            Description(x => x.WithTags("Ratings"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(GetRecipeRatingsRequest req, CancellationToken ct)
        {
            var query = new GetRecipeRatingsQuery(req.RecipeId);
            var result = await _mediator.Send(query);
            await Send.OkAsync(result); 
        }
    }
}
