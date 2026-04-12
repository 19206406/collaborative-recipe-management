using BuildingBlocks.Messaging.Events.RatingEvents;
using BuildingBlocks.Messaging.RabbitMQ;
using Microsoft.Extensions.Options;
using Recipe.API.Repositories.RecipeRepository;

namespace Recipe.API.Consumers
{
    public class RatingCreateAndUpdateConsumer : RabbitMQConsumer<RatingCreateAndUpdateEvent>
    {
        private readonly ILogger<RatingCreateAndUpdateConsumer> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        protected override string QueueName => "rating.created"; 

        public RatingCreateAndUpdateConsumer(
            IOptions<RabbitMQSettings> settings, 
            ILogger<RatingCreateAndUpdateConsumer> logger,
            IServiceScopeFactory scopeFactory)
            : base(settings, logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ProcessMessageAsync(RatingCreateAndUpdateEvent message)
        {
            using var scope = _scopeFactory.CreateScope();

            var recipeRepository = scope.ServiceProvider
                .GetRequiredService<IRecipeRepository>(); 

            var recipe = await recipeRepository.GetRecipeOnly(message.RecipeId);

            if (recipe is null)
            {
                _logger.LogWarning(
                    "Receta con ID {RecipeId} no encontrada. Mensaje descartado.",
                    message.RecipeId);
                return;
            }

            if (message.IsToUpdate) 
            {
                var updateAverage = (recipe.AverageRating * recipe.RatingCount - message.OldRating + message.Rating) / recipe.RatingCount;
                recipe.AverageRating = updateAverage;
                recipe.UpdatedAt = DateTime.UtcNow;

                await recipeRepository.UpdateRatingRecipeOnly();
                _logger.LogInformation("Evento RatingAndUpdateConsumer consumido correctamente para actualizar rating desde recipe " +
                    "y cambio de la puntuación promedio de la receta"); 
            }
            else 
            {
                var newAverage = (recipe.AverageRating * recipe.RatingCount + message.Rating) / (recipe.RatingCount + 1);
                recipe.RatingCount += 1;
                recipe.AverageRating = newAverage;
                recipe.UpdatedAt = DateTime.UtcNow;

                await recipeRepository.UpdateRatingRecipeOnly();
                _logger.LogInformation("Evento RatingAndUpdateConsumer consumido correctamente para crear rating desde recipe " +
                    "y cambio de la puntuación promedio de la receta");
            }

            await Task.CompletedTask; 
        }
    }
}