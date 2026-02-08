namespace Rating.API.Features.CreateRating
{
    public record CreateRatingResponse(int Id, int UserId, int RecipeId, int Rating, string? Comment, DateTime CreatedAt); 
}
