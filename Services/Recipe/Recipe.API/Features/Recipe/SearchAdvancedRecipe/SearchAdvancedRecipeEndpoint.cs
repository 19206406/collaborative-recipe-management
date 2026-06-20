using FastEndpoints;
using MediatR;

namespace Recipe.API.Features.Recipe.SearchAdvancedRecipe
{
    public record SearchAdvancedRecipeRequest(string? Title, int? PrepTimeMinutes, int? CookTimeMinutes, string? Difficulty,
        int? Servings, string? SortBy, List<string>? Tags, bool SortDescending = true); 
    public class SearchAdvancedRecipeEndpoint : Endpoint<SearchAdvancedRecipeRequest, List<SearchAdvancedRecipeResponse>>
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
                x.Summary = "Advanced recipe search";
                x.Description = "Searches for recipes using multiple advanced filters such as name, tags, ingredients, and more.";
            });
            Description(x => x.WithTags("Recipes"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(SearchAdvancedRecipeRequest req, CancellationToken ct)
        {
            var query = new SearchAdvancedRecipeQuery(req.Title, req.PrepTimeMinutes, req.CookTimeMinutes,
                req.Difficulty, req.Servings, req.SortBy, req.SortDescending, req.Tags);

            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
