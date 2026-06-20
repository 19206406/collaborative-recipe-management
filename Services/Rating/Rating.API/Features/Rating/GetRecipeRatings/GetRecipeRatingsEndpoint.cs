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
                x.Summary = "Get all ratings for a recipe";
                x.Description = "Returns a list of all ratings submitted for a specific recipe.";
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
