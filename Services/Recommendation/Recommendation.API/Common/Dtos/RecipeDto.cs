namespace Recommendation.API.Common.Dtos
{
    public record ResponseRecipe(int Id, int UserId, string Title, string Description, int PrepTimeMinutes,
        int CookTimeMinutes, int Difficulty, int Servings, string ImageUrl, decimal AverageRating, int RatingCount, DateTime CreatedAt);
    public record ResponseIngredient(int Id, string Name, decimal Quantity, string Unit, int DisplayOrder);
    public record ResponseStep(int Id, int StepNumber, string Instruction);
    public record ResponseTag(int Id, string Tag);
    public record RecipeDto(ResponseRecipe recipe, List<ResponseIngredient> Ingredients,
        List<ResponseStep> Steps, List<ResponseTag> Tags); 
}
