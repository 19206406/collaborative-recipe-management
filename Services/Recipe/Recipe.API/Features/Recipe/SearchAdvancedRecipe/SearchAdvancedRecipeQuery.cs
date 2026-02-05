using BuildingBlocks.CQRS;

namespace Recipe.API.Features.Recipe.SearchAdvancedRecipe
{
    public record SearchAdvancedRecipeQuery(string? Title, int? PrepTimeMinutes, int? CookTimeMinutes, int? Difficulty,
        int? Servings, string? SortBy, bool SortDescending) : IQuery<SearchAdvancedRecipeResponse>; 
}
