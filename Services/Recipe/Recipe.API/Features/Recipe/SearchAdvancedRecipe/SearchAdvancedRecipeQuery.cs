using BuildingBlocks.CQRS;

namespace Recipe.API.Features.Recipe.SearchAdvancedRecipe
{
    public record SearchAdvancedRecipeQuery(string? Title, int? PrepTimeMinutes, int? CookTimeMinutes, string? Difficulty,
        int? Servings, string? SortBy, bool SortDescending, List<string>? Tags) : IQuery<SearchAdvancedRecipeResponse>; 
}
