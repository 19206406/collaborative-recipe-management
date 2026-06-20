using FastEndpoints;
using MediatR;

namespace Rating.API.Features.Rating.GetAEspecificRating
{
    public record GetAEspecificRatingRequest(int UserId, int RecipeId); 

    public class GetAEspecificRatingEndpoint : Endpoint<GetAEspecificRatingRequest, GetAEspecificRatingResponse>
    {
        private readonly IMediator _mediator;

        public GetAEspecificRatingEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/api/ratings/user/{userId}/recipe/{recipeId}");
            Summary(x =>
            {
                x.Summary = "Get user rating for a recipe";
                x.Description = "Returns the rating that a specific user has given to a specific recipe.";
            });
            Description(x => x.WithTags("Ratings"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(GetAEspecificRatingRequest req, CancellationToken ct)
        {
            var query = new GetAEspecificRatingQuery(req.UserId, req.RecipeId);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
        
    }
}
