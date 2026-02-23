using FastEndpoints;
using MediatR;
using Recommendation.API.Common.Dtos;

namespace Recommendation.API.Features.Recommendation.GetSimilarRecipes
{
    public record GetSimilarRecipesRequest(int recipeId); 

    public class GetSimilarRecipesEndpoint : Endpoint<GetSimilarRecipesRequest, List<RecipeDto>>
    {
        private readonly IMediator _mediator;

        public GetSimilarRecipesEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("api/recommendations/similar/{recipeId}");
            Summary(x =>
            {
                x.Summary = "Recetas similares";
                x.Description = "Obtiene recetas similares dado una receta que le guste al usuario";
            });
            Description(x => x.WithTags("Recommendations")); 
        }

        public override Task HandleAsync(GetSimilarRecipesRequest req, CancellationToken ct)
        {
            var query = new GetSimilarRecipesQuery(req.recipeId); 
        }
    }
}
