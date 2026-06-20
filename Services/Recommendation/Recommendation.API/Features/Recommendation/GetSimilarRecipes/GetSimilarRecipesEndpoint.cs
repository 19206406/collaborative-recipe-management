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
                x.Summary = "Get similar recipes";
                x.Description = "Returns a list of recipes similar to the specified recipe, based on shared tags, ingredients, or category.";
            });
            Description(x => x.WithTags("Recommendations"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(GetSimilarRecipesRequest req, CancellationToken ct)
        {
            var query = new GetSimilarRecipesQuery(req.recipeId);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
