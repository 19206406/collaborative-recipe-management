using FastEndpoints;
using MediatR;

namespace Recipe.API.Features.Recipe.SearchAdvancedRecipe
{
    public record SearchAdvancedRecipeRequest(string? Title, int? PrepTimeMinutes, int? CookTimeMinutes, int? Difficulty,
        int? Servings, string? SortBy, bool SortDescending); 
    public class SearchAdvancedRecipeEndpoint : Endpoint<SearchAdvancedRecipeRequest, SearchAdvancedRecipeResponse>
    {
        private readonly IMediator _mediator;

        public SearchAdvancedRecipeEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("api/recipes/search");
            Summary(x =>
            {
                x.Summary = "Búsqueda avanzada de recetas";
                x.Description = "Permite buscar recetas utilizando múltiples filtros avanzados";
            });
            Description(x => x.WithTags("Recipes"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(SearchAdvancedRecipeRequest req, CancellationToken ct)
        {
            var query = new SearchAdvancedRecipeQuery(req.Title, req.PrepTimeMinutes, req.CookTimeMinutes,
                req.Difficulty, req.Servings, req.SortBy, req.SortDescending);

            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
