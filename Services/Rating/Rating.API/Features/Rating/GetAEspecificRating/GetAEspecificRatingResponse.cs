namespace Rating.API.Features.Rating.GetAEspecificRating
{
    public record GetAEspecificRatingResponse(int UserId, int RecipeId, int Rating, string? Comment, DateTime CreatedAt, DateTime UpdatedAt); 
}
