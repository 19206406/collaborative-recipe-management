using FastEndpoints;
using MediatR;

namespace Recipe.API.Features.Recipe.GetRecipesByIngredients
{
    public record GetRecipesByIngredientsRequest(List<string> Ingredients); 
    public class GetRecipesByIngredientsEndpoint : Endpoint<GetRecipesByIngredientsRequest, List<GetRecipesByIngredientsResponse>>
    {
        private readonly IMediator _mediator;

        public GetRecipesByIngredientsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/api/recipes/by-ingredients");
            Summary(x =>
            {
                x.Summary = "Get recipes by ingredients";
                x.Description = "Returns a list of recipes that match a provided list of ingredients.";
            });
            Description(x => x.WithTags("Recipes"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(GetRecipesByIngredientsRequest req, CancellationToken ct)
        {
            var query = new GetRecipesByIngredientsQuery(req.Ingredients);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
