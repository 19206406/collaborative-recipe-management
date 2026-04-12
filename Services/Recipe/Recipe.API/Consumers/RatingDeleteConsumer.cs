using BuildingBlocks.Messaging.Events.RatingEvents;
using BuildingBlocks.Messaging.RabbitMQ;
using Microsoft.Extensions.Options;
using Recipe.API.Repositories.RecipeRepository;

namespace Recipe.API.Consumers
{
    public class RatingDeleteConsumer : RabbitMQConsumer<RatingDeleteEvent>
    {
        private readonly ILogger<RatingDeleteConsumer> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        protected override string QueueName => "rating.deleted";

        public RatingDeleteConsumer(
            IOptions<RabbitMQSettings> settings,
            ILogger<RatingDeleteConsumer> logger, 
            IServiceScopeFactory scopeFactory
            ) : base(settings, logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ProcessMessageAsync(RatingDeleteEvent message)
        {
            using var scope = _scopeFactory.CreateScope();

            var recipeRepository = scope.ServiceProvider
                .GetRequiredService<IRecipeRepository>(); 

            var recipe = await recipeRepository.GetRecipe(message.RecipeId);

            if (recipe is null)
            {
                _logger.LogWarning("Receta con ID {RecipeId} no encontrada. Mensaje descartado", 
                    message.RecipeId);
                return; 
            }

            if (recipe.RatingCount <= 1)
            {
                recipe.AverageRating = 0;
                recipe.RatingCount = 0; 
            }
            else 
            {
                var updateAverage = (recipe.AverageRating * recipe.RatingCount - message.Rating) / (recipe.RatingCount - 1);
                recipe.RatingCount -= 1;
                recipe.AverageRating = updateAverage;
                recipe.UpdatedAt = DateTime.UtcNow; 
            }

            await recipeRepository.UpdateRatingRecipeOnly();
            _logger.LogInformation("Evento RatingDeleteEvent consumido correctamente para eliminar rating desde recipe " +
                "y cambio de la puntución promedio de la receta"); 

            await Task.CompletedTask; 
        }
    }
}
