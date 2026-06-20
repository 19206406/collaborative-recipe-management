using BuildingBlocks.Jwt.Claims;
using FastEndpoints;
using MediatR;

namespace Rating.API.Features.Rating.CreateRating
{
    public record CreateRatingRequest(int RecipeId, int Rating, string? Comment); 

    public class CreateRatingEndpoint : Endpoint<CreateRatingRequest, CreateRatingResponse>
    {
        private readonly IMediator _mediator;

        public CreateRatingEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("api/ratings/only");
            Summary(x =>
            {
                x.Summary = "Create a rating (standalone)";
                x.Description = "Creates a rating for a specific recipe without triggering any additional side effects such as recommendation updates.";
            });
            Description(x => x.WithTags("Ratings")); 
        }

        public override async Task HandleAsync(CreateRatingRequest req, CancellationToken ct)
        {
            var userId = HttpContext.User.GetUserId(); 

            var command = new CreateRatingCommand(userId, req.RecipeId, req.Rating, req.Comment);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result); 
        }
    }
}
