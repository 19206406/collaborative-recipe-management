namespace Recommendation.API.Common.Dtos
{
    public record UserRatingDto(int Id, int UserId, int RecipeId, int Rating, string? Comment, DateTime CreatedAt); 
}
