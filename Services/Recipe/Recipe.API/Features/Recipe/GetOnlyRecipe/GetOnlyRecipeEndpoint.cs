using FastEndpoints;
using MediatR;

namespace Recipe.API.Features.Recipe.GetOnlyRecipe
{
    public record GetOnlyRecipeRequest(int Id); 
    public class GetOnlyRecipeEndpoint : Endpoint<GetOnlyRecipeRequest, GetOnlyRecipeResponse>
    {
        private readonly IMediator _mediator;

        public GetOnlyRecipeEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/api/recipes/get-only-recipe/{Id}");
            Summary(x =>
            {
                x.Summary = "Obtener solo la receta";
                x.Description = "Obtener solo una receta teniendo solo la información basica";
            });
            Description(x => x.WithTags("Recipes")); 
        }

        public override async Task HandleAsync(GetOnlyRecipeRequest req, CancellationToken ct)
        {
            var query = new GetOnlyRecipeQuery(req.Id);
            var result = await _mediator.Send(query);

            await Send.OkAsync(); 
        }
    }
}
