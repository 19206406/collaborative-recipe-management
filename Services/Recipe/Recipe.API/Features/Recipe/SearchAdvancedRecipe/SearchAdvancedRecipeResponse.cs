namespace Recipe.API.Features.Recipe.SearchAdvancedRecipe
{
    public record SearchAdvancedRecipeResponse(int Id, int UserId, string Title, string Description, int PrepTimeMinutes, int CookTimeMinutes, 
        string Difficulty, int Servings, string ImageUrl, decimal AverageRating, int RatingCount, DateTime CreatedAt, DateTime UpdatedAt); 
}
