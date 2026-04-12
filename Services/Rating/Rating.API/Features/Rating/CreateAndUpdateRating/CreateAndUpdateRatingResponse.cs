namespace Rating.API.Features.Rating.CreateAndUpdateRating
{
    public record CreateAndUpdateRatingResponse(int Id, int UserId, int RecipeId, 
        int Rating, int OldRating, string? Comment, DateTime CreatedAt, DateTime UpdatedAt); 
}
