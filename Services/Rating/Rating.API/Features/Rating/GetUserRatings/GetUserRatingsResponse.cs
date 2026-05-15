namespace Rating.API.Features.Rating.GetUserRatings
{
    public record GetUserRatingsResponse(int Id, int UserId, int RecipeId, int Rating, string? Comment, DateTime CreatedAt, DateTime UpdatedAt); 
}

