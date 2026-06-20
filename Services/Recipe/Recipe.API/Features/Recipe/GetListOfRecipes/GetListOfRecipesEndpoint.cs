using FastEndpoints;
using MediatR;

namespace Recipe.API.Features.Recipe.GetListOfRecipes
{
    public record SearchAdvancedRecipe(string? Title, int? PrepTimeMinutes, int? CookTimeMinutes, string? Difficulty,
        int? Servings, string? SortBy, bool SortDescending);

    public record GetListOfRecipesRequest(string? Title, int? PrepTimeMinutes, int? CookTimeMinutes, string? Difficulty,
        int? Servings, string? SortBy, bool SortDescending, int PageNumber = 1, int PageSize = 10); 

    public class GetListOfRecipesEndpoint : Endpoint<GetListOfRecipesRequest, GetListOfRecipesResponse>
    {
        private readonly IMediator _mediator;

        public GetListOfRecipesEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/api/recipes");
            Summary(x =>
            {
                x.Summary = "Get recipes";
                x.Description = "Returns a paginated list of all recipes. Supports filtering by multiple criteria.";
            });
            Description(x => x.WithTags("Recipes"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(GetListOfRecipesRequest req, CancellationToken ct)
        {
            var criteria = new SearchAdvancedRecipe(
                req.Title,
                req.PrepTimeMinutes,
                req.CookTimeMinutes,
                req.Difficulty,
                req.Servings,
                req.SortBy,
                req.SortDescending
            );

            var query = new GetListOfRecipesQuery(req.PageNumber, req.PageSize, criteria);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
