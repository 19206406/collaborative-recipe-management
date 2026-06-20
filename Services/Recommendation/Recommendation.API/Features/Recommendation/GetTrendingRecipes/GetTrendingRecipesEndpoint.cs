using FastEndpoints;
using MediatR;
using Recommendation.API.Common.Dtos;

namespace Recommendation.API.Features.Recommendation.GetTrendingRecipes
{
    public class GetTrendingRecipesEndpoint : EndpointWithoutRequest<List<RecipeDto>>
    {
        private readonly IMediator _mediator;

        public GetTrendingRecipesEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        } 

        public override void Configure()
        {
            Get("/api/recommendations/trending");
            Summary(x =>
            {
                x.Summary = "Get top trending recipes";
                x.Description = "Returns the top 20 recipes with the highest trending score based on recent ratings and activity.";
            });
            Description(x => x.WithTags("Recommendations"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var query = new GetTrendingRecipesQuery();
            var recipes = await _mediator.Send(query);

            await Send.OkAsync(recipes); 
        }
    }
}
