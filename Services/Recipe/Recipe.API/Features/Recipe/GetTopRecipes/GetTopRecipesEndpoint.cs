using FastEndpoints;
using MediatR;

namespace Recipe.API.Features.Recipe.GetTopRecipes
{
    public class GetTopRecipesEndpoint : EndpointWithoutRequest<List<GetTopRecipesResponse>>
    {
        private readonly IMediator _mediator;

        public GetTopRecipesEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/api/recipes/trending");
            Summary(x =>
            {
                x.Summary = "Recetas trending"; 
                x.Description = "Recetas trending"; 
            });
            Description(x => x.WithTags("Recipes"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var query = new GetTopRecipesQuery();
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
