using FastEndpoints;
using MediatR;

namespace Notification.API.Features.Notification.AddReceivedRatingNotification
{
    public record AddReceivedRatingNotificationRequest(int RecipeId, int RatingValue, int UserId); // int UserId (quien calificó) 

    public class AddReceivedRatingNotificationEndpoint : Endpoint<AddReceivedRatingNotificationRequest, AddReceivedRatingNotificationResponse>
    {
        private readonly IMediator _mediator;

        public AddReceivedRatingNotificationEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/api/notifications/rating-received");
            Summary(x =>
            {
                x.Summary = "Receive rating notification (internal)";
                x.Description = "Internal endpoint used by the Rating Service to create a notification when a recipe receives a new rating.";
            });
            Description(x => x.WithTags("Notifications"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(AddReceivedRatingNotificationRequest req, CancellationToken ct)
        {
            var command = new AddReceivedRatingNotificationCommand(req.RecipeId, req.RatingValue, req.UserId);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result); 
        }
    }
}
