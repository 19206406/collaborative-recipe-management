using FastEndpoints;
using MediatR;

namespace Rating.API.Features.Rating.GetUserRatings
{
    public record GetUserRatingsRequest(int UserId); 

    public class GetUserRatingsEndpoint : Endpoint<GetUserRatingsRequest, List<GetUserRatingsResponse>>
    {
        private readonly IMediator _mediator;

        public GetUserRatingsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/api/ratings/user/{userId}");
            Summary(x =>
            {
                x.Summary = "Get ratings by user";
                x.Description = "Returns a list of all ratings submitted by a specific user.";
            });
            Description(x => x.WithTags("Ratings"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(GetUserRatingsRequest req, CancellationToken ct)
        {
            var query = new GetUserRatingsQuery(req.UserId);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
