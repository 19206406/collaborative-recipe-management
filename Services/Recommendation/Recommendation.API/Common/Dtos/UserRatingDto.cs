namespace Recommendation.API.Common.Dtos
{
    //public record RatingResponse(int Id, int UserId, int RecipeId, int Rating, string? Comment, DateTime CreatedAt);
    public record UserRatingDto(int Id, int UserId, int RecipeId, int Rating, string? Comment, DateTime CreatedAt); 
}
