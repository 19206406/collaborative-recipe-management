namespace Recipe.API.Features.Recipe.GetTopRecipes
{
    public record ResponseRecipe(int Id, int UserId, string Title, string Description, int PrepTimeMinutes,
        int CookTimeMinutes, string Difficulty, int Servings, string ImageUrl, decimal AverageRating, int RatingCount, DateTime CreatedAt, DateTime UpdatedAt);
    public record ResponseIngredient(int Id, string Name, decimal Quantity, string Unit, int DisplayOrder);
    public record ResponseStep(int Id, int StepNumber, string Instruction);
    public record ResponseTag(int Id, string Tag);
    public record GetTopRecipesResponse(int Id, int UserId, string Title, string Description, int PrepTimeMinutes,
        int CookTimeMinutes, string Difficulty, int Servings, string ImageUrl, decimal AverageRating,
        int RatingCount, DateTime CreatedAt, DateTime UpdatedAt, 
        List<ResponseIngredient> Ingredients, List<ResponseStep> Steps, List<ResponseTag> RecipeTags); 
}
