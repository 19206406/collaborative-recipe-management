using FastEndpoints;
using MediatR;

namespace Recipe.API.Features.Recipe.GetRecipe
{
    public record GetRecipeRequest(int Id); 

    public class GetRecipeEndpoint : Endpoint<GetRecipeRequest, GetRecipeResponse>
    {
        private readonly IMediator _mediator;

        public GetRecipeEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public const string Route = "/api/recipes/{id}"; 

        public override void Configure()
        {
            Get(Route);
            Summary(x =>
            {
                x.Summary = "Get recipe with details";
                x.Description = "Returns a single recipe with its full details, including steps, ingredients, tags, and ratings.";
            });
            Description(x => x.WithTags("Recipes"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(GetRecipeRequest req, CancellationToken ct)
        {
            var query = new GetRecipeQuery(req.Id);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
