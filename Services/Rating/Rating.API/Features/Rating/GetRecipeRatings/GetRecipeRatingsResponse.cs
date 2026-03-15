namespace Rating.API.Features.Rating.GetRecipeRatings
{
    public record RatingResponse(int Id, int UserId, int RecipeId, int Rating, string? Comment, DateTime CreatedAt, DateTime UpdatedAt); 
    public record GetRecipeRatingsResponse(List<RatingResponse> Ratings); 
}
