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
                x.Summary = "Uso interno para el servicio de rating";
                x.Description = "Crea una nueva notificación con información del servicio de rating";
            });
            Description(x => x.WithTags("Notifications"));
        }

        public override async Task HandleAsync(AddReceivedRatingNotificationRequest req, CancellationToken ct)
        {
            var command = new AddReceivedRatingNotificationCommand(req.RecipeId, req.RatingValue, req.UserId);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result); 
        }
    }
}
