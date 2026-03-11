namespace Recipe.API.Features.Recipe.UpdateRecipe
{
    public record CollectionSummary(int created, int updated, int deleted); 
    public record UpdateRecipeResponse(
        int Id, 
        string Title, 
        string Description, 
        int PrepTimeMinutes, 
        int CookTimeMinutes, 
        string Difficulty, 
        int Servings, 
        string ImageUrl, 
        DateTime UpdatedAt,
        RecipeUpdateSummary Summary
        ); 
}
