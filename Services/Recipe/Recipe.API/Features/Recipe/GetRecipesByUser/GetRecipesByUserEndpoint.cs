using FastEndpoints;
using MediatR;

namespace Recipe.API.Features.Recipe.GetRecipesByUser
{
    public record GetRecipeByUserRequest(int UserId); 
    public class GetRecipesByUserEndpoint : Endpoint<GetRecipeByUserRequest, List<GetRecipesByUserResponse>>
    {
        private readonly IMediator _mediator;

        public GetRecipesByUserEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/api/recipes/user/{userId}");
            Summary(x =>
            {
                x.Summary = "Get recipes by user";
                x.Description = "Returns all recipes created by a specific user.";
            });
            Description(x => x.WithTags("Recipes"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(GetRecipeByUserRequest req, CancellationToken ct)
        {
            var query = new GetRecipesByUserQuery(req.UserId);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
