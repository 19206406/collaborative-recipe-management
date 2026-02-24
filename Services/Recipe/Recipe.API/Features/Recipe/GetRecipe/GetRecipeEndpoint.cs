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
                x.Summary = "Obtener una receta";
                x.Description = "Obtener una receta con detalles";
            });
            Description(x => x.WithTags("Recipes")); 
        }

        public override async Task HandleAsync(GetRecipeRequest req, CancellationToken ct)
        {
            var query = new GetRecipeQuery(req.Id);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
