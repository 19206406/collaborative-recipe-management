namespace Rating.API.Features.Rating.CreateRating
{
    public record CreateRatingResponse(int Id, int UserId, int RecipeId, int Rating, string? Comment, DateTime CreatedAt); 
}
