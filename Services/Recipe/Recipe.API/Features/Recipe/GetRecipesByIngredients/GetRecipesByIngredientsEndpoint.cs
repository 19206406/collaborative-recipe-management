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
                x.Summary = "Obtener recetas";
                x.Description = "Obtener recetas por medio de una lista de ingredientes";
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
