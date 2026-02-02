using BuildingBlocks.Pagination;
using FastEndpoints;
using MediatR;

namespace Recipe.API.Features.Recipe.GetListOfRecipes
{
    public class GetListOfRecipesEndpoint : Endpoint<PaginationRequest, GetListOfRecipesResponse>
    {
        private readonly IMediator _mediator;

        public GetListOfRecipesEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("api/recipes");
            Summary(x =>
            {
                x.Summary = "Obtener recetas";
                x.Description = "Obtiene todas las recetas con paginación y filtros incluidos";
            });
            Description(x => x.WithTags("Recipes")); 
        }

        public override async Task HandleAsync(PaginationRequest req, CancellationToken ct)
        {
            var query = new GetListOfRecipesQuery(req.PageNumber, req.PageSize);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
