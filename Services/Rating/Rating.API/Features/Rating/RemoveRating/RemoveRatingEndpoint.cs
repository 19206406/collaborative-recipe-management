using BuildingBlocks.Jwt.Claims;
using BuildingBlocks.Messaging.Events.RatingEvents;
using BuildingBlocks.Messaging.RabbitMQ;
using FastEndpoints;
using MediatR;

namespace Rating.API.Features.Rating.RemoveRating
{
    public record RemoveRatingRequest(int Id); 

    public class RemoveRatingEndpoint : Endpoint<RemoveRatingRequest>
    {
        private readonly IMediator _mediator;
        private readonly IMessagePublisher _messagePublisher;

        public RemoveRatingEndpoint(IMediator mediator, IMessagePublisher messagePublisher)
        {
            _mediator = mediator;
            _messagePublisher = messagePublisher;
        }

        public override void Configure()
        {
            Delete("api/ratings/{id}");
            Summary(x =>
            {
                x.Summary = "Elimina una calificación existente.";
                x.Description = "Elimina una calificación existente. Requiere autenticación.";
            });
            Description(x => x.WithTags("Ratings")); 
        }

        public override async Task HandleAsync(RemoveRatingRequest req, CancellationToken ct)
        {
            // verificar autenticación y autorización para realizar esta acción 
            var userId = HttpContext.User.GetUserId(); 

            var command = new RemoveRatingCommand(req.Id, userId);
            var result = await _mediator.Send(command);

            await _messagePublisher.PublishAsync("rating.deleted", new RatingDeleteEvent
            {
                Id = result.Id, 
                RecipeId = req.Id,
                UserId = userId, 
                Rating = result.Rating,
            });  

            await Send.NoContentAsync(); 
        }
    }
}
