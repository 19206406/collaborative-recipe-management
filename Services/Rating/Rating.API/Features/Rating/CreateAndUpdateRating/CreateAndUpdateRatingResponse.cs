namespace Rating.API.Features.Rating.CreateAndUpdateRating
{
    public record CreateAndUpdateRatingResponse(int Id, int UserId, int RecipeId, int Rating, string? Comment, DateTime CreatedAt); 
}
