using BuildingBlocks.Jwt.Claims;
using FastEndpoints;
using MediatR;
using Recommendation.API.Common.Dtos;

namespace Recommendation.API.Features.Recommendation.RecommendationsByIngredients
{
    public record RecommendationsByIngredientsRequest(int UserId, List<string> Ingredients); 

    public class RecommendationsByIngredientsEndpoint : Endpoint<RecommendationsByIngredientsRequest, List<IngredientMatchDto>>
    {
        private readonly IMediator _mediator;

        public RecommendationsByIngredientsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/api/recommendations/by-ingredients");
            Summary(x =>
            {
                x.Summary = "Get recipe recommendations by ingredients";
                x.Description = "Returns recipe recommendations based on a provided list of ingredients. Only recipes that meet a minimum match percentage are included in the results.";
            });
            Description(x => x.WithTags("Recommendations"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(RecommendationsByIngredientsRequest req, CancellationToken ct)
        {
            var command = new RecommendationsByIngredientsCommand(req.UserId, req.Ingredients);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result); 
        }
    }
}
