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
                x.Summary = "Get average rating for a recipe";
                x.Description = "Returns the average rating score for the recipe identified by the provided ID.";
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
