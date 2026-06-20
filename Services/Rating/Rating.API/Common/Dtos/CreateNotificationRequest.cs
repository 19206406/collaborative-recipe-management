namespace Rating.API.Common.Dtos
{
    public record CreateNotificationRequest(int RecipeId, int RatingValue, int UserId);
}
