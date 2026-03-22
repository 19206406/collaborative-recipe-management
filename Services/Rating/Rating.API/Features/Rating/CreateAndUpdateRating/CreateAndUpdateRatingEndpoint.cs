using BuildingBlocks.Jwt.Claims;
using BuildingBlocks.Messaging.Events.RatingEvents;
using BuildingBlocks.Messaging.RabbitMQ;
using FastEndpoints;
using MediatR;

namespace Rating.API.Features.Rating.CreateAndUpdateRating
{
    public record CreateAndUpdateRatingRequest(int Id, int RecipeId, int Rating, string? Comment, bool IsToUpdate = false); 
    public class CreateAndUpdateRatingEndpoint : Endpoint<CreateAndUpdateRatingRequest, CreateAndUpdateRatingResponse>
    {
        private readonly IMediator _mediator;
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<CreateAndUpdateRatingEndpoint> _logger;

        public CreateAndUpdateRatingEndpoint(IMediator mediator, IMessagePublisher messagePublisher, ILogger<CreateAndUpdateRatingEndpoint> logger)
        {
            _mediator = mediator;
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        public override void Configure()
        {
            Post("/api/ratings");
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

            try
            {
                //publicar evento de creación de evento o de actualizaciòn 
                await _messagePublisher.PublishAsync("rating.created", new RatingCreateAndUpdateEvent
                {
                    RatingId = req.Id,
                    RecipeId = req.RecipeId,
                    UserId = userId,
                    Rating = req.Rating,
                    Comment = req.Comment,
                    IsToUpdate = req.IsToUpdate,
                    PublishedAt = DateTime.UtcNow
                });

                _logger.LogInformation("Evento CreateAndUpdateRating creado exitosamente"); 
            }
            catch (Exception eventEx)
            {
                _logger.LogError(eventEx, "Rating guardado con cambios exitosamente en base de datos, pero evento CreateAndUpdateRating No publicado"); 
            }

            await Send.OkAsync(result);
        }
    }
}
