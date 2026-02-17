using BuildingBlocks.Messaging.Events.RatingEvents;
using BuildingBlocks.Messaging.RabbitMQ;
using Microsoft.Extensions.Options;
using Recipe.API.Repositories.RecipeRepository;

namespace Recipe.API.Consumers
{
    public class RatingDeleteConsumer : RabbitMQConsumer<RatingDeleteEvent>
    {
        private readonly IRecipeRepository _recipeRepository;

        protected override string QueueName => "rating.deleted";

        public RatingDeleteConsumer(
            IOptions<RabbitMQSettings> settings,
            ILogger<RatingDeleteConsumer> logger, 
            IRecipeRepository recipeRepository) : base(settings, logger)
        {
            _recipeRepository = recipeRepository;
        }

        protected override async Task ProcessMessageAsync(RatingDeleteEvent message)
        {
            Logger.LogInformation("Rating eliminado por el usuario con Id: {userId}", message.UserId);

            var recipe = await _recipeRepository.GetRecipe(message.RecipeId);

            var updateAverage = (recipe.AverageRating * recipe.RatingCount - message.Rating) / (recipe.RatingCount - 1);
            recipe.RatingCount -= 1;
            recipe.AverageRating = updateAverage;

            await _recipeRepository.UpdateRecipeOnly(recipe);

            await Task.CompletedTask; 
        }
    }
}
