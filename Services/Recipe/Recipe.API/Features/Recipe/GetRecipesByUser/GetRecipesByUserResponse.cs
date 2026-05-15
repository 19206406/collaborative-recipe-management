namespace Recipe.API.Features.Recipe.GetRecipesByUser
{
    public record GetRecipesByUserResponse(int Id, int UserId, string Title, string Description, int PrepTimeMinutes,
        int CookTimeMinutes, string Difficulty, int Servings, string ImageUrl, decimal AverageRating, int RatingCount, DateTime CreatedAt, DateTime UpdatedAt); 
}
