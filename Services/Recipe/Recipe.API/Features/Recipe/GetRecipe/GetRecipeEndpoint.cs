using FastEndpoints;
using MediatR;

namespace Recipe.API.Features.Recipe.GetRecipe
{
    public record GetRecipeRequest(); 

    public class GetRecipeEndpoint : Endpoint<GetRecipeRequest, GetRecipeResponse>
    {
        private readonly IMediator _mediator;

        public GetRecipeEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("api/recipes/{id}");
            Summary(x =>
            {
                x.Summary = "Obtener una receta";
                x.Description = "Obtener una receta con detalles";
            });
            Description(x => x.WithTags("Recipes")); 
        }

        public override async Task HandleAsync(GetRecipeRequest req, CancellationToken ct)
        {
            var query = new GetRecipeQuery();
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
