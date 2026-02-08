namespace Rating.API.Features.CreateAndUpdateRating
{
    public record CreateAndUpdateRatingResponse(int Id, int UserId, int RecipeId, int Rating, string? Comment, DateTime CreatedAt); 
}
