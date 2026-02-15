using BuildingBlocks.Pagination;
using FastEndpoints;
using MediatR;

namespace Recipe.API.Features.Recipe.GetListOfRecipes
{
    public record SearchAdvancedRecipe(string? Title, int? PrepTimeMinutes, int? CookTimeMinutes, int? Difficulty,
        int? Servings, string? SortBy, bool SortDescending);

    public record GetListOfRecipesRequest(PaginationRequest Pagination, SearchAdvancedRecipe criteria); 

    public class GetListOfRecipesEndpoint : Endpoint<GetListOfRecipesRequest, GetListOfRecipesResponse>
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

        public override async Task HandleAsync(GetListOfRecipesRequest req, CancellationToken ct)
        {
            var query = new GetListOfRecipesQuery(req.Pagination.PageNumber, req.Pagination.PageSize, req.criteria);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
