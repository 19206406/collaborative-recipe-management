namespace Notification.API.Common.Dtos
{
    public record Recipe(int Id, int UserId, string Title, string Description, int PrepTimeMinutes,
        int CookTimeMinutes, string Difficulty, int Servings, string ImageUrl, decimal AverageRating, int RatingCount, DateTime CreatedAt, DateTime UpdatedAt);
    
    public record RecipeDto(Recipe Recipe);
}
