using BuildingBlocks.CQRS;

namespace Notification.API.Features.Notification.AddReceivedRatingNotification
{
    public record AddReceivedRatingNotificationCommand(int RecipeId, int RatingValue, int UserId) 
        : ICommand<AddReceivedRatingNotificationResponse>; 
}
