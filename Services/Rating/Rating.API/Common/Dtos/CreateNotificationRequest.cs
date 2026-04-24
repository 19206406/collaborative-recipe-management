namespace Rating.API.Common.Dtos
{
    public record CreateNotificationRequest(int RecipeId, int RatingValue, int UserId);

    //public class CreateNotificationRequest
    //{
    //    public int RecipeId { get; set; }
    //    public int RatingValue { get; set; }    
    //    public int UserId { get; set; }
    //}
}
