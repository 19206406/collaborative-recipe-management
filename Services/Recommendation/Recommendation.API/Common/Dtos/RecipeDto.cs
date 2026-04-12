namespace Recommendation.API.Common.Dtos
{
    public record ResponseIngredient(int Id, string Name, decimal Quantity, string Unit, int DisplayOrder);
    public record ResponseTag(int Id, string Tag);

    //public record RecipeDto(RecipeAndIngredients recipe); 

    public record RecipeDto(int Id, int UserId, string Title, string Description, int PrepTimeMinutes,
        int CookTimeMinutes, string Difficulty, int Servings, string ImageUrl, decimal AverageRating,
        int RatingCount, DateTime CreatedAt, DateTime UpdatedAt, List<ResponseIngredient> Ingredients, List<ResponseTag> RecipeTags); 
}
