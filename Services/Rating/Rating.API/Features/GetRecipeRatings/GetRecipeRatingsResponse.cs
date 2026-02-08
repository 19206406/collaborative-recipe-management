namespace Rating.API.Features.GetRecipeRatings
{
    public record RatingResponse(int Id, int UserId, int RecipeId, int Rating, string? Comment, DateTime CreatedAt); 
    public record GetRecipeRatingsResponse(List<RatingResponse> Ratings); 
}
