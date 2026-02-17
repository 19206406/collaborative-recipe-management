using BuildingBlocks.Jwt.Claims;
using BuildingBlocks.Messaging.Events.RatingEvents;
using BuildingBlocks.Messaging.RabbitMQ;
using FastEndpoints;
using MediatR;

namespace Rating.API.Features.Rating.CreateAndUpdateRating
{
    public record CreateAndUpdateRatingRequest(int Id, int RecipeId, int Rating, string? Comment, bool IsToUpdate); 
    public class CreateAndUpdateRatingEndpoint : Endpoint<CreateAndUpdateRatingRequest, CreateAndUpdateRatingResponse>
    {
        private readonly IMediator _mediator;
        private readonly IMessagePublisher _messagePublisher;

        public CreateAndUpdateRatingEndpoint(IMediator mediator, IMessagePublisher messagePublisher)
        {
            _mediator = mediator;
            _messagePublisher = messagePublisher;
        }

        public override void Configure()
        {
            Put("api/ratings");
            Summary(x =>
            {
                x.Summary = "Crear o actualizar una calificación";
                x.Description = "Permite crear o actualizar una calificación para una receta específica.";
            });
            Description(x => x.WithTags("Ratings"));
        }

        public override async Task HandleAsync(CreateAndUpdateRatingRequest req, CancellationToken ct)
        {
            // id del usuario del jwt  
            var userId = HttpContext.User.GetUserId(); 

            var command = new CreateAndUpdateRatingCommand(req.Id, userId, req.RecipeId, req.Rating, req.Comment, req.IsToUpdate);
            var result = await _mediator.Send(command);

            // publicar evento de creación de evento 
            await _messagePublisher.PublishAsync("rating.created", new RatingCreateAndUpdateEvent
            {
                RatingId = result.Id,
                RecipeId = req.RecipeId,
                UserId = userId,
                Rating = req.Rating,
                Comment = req.Comment,
                IsToUpdate = req.IsToUpdate, 
                PublishedAt = DateTime.UtcNow
            }); 

            await Send.OkAsync(result); 
        }
    }
}
