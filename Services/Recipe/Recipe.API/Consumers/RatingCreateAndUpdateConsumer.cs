using BuildingBlocks.Messaging.Events.RatingEvents;
using BuildingBlocks.Messaging.RabbitMQ;
using Microsoft.Extensions.Options;
using Recipe.API.Repositories.RecipeRepository;

namespace Recipe.API.Consumers
{
    public class RatingCreateAndUpdateConsumer : RabbitMQConsumer<RatingCreateAndUpdateEvent>
    {
        private readonly IRecipeRepository _recipeRepository;

        protected override string QueueName => "rating.created"; 

        public RatingCreateAndUpdateConsumer(
            IOptions<RabbitMQSettings> settings, 
            ILogger<RatingCreateAndUpdateConsumer> logger,
            IRecipeRepository recipeRepository)
            : base(settings, logger)
        {
            _recipeRepository = recipeRepository;
        }

        protected override async Task ProcessMessageAsync(RatingCreateAndUpdateEvent message)
        {
            Logger.LogInformation("Nuevo rating creado o actualizado por userId: {userId}", message.UserId);

            var recipe = await _recipeRepository.GetRecipe(message.RecipeId);

            if (message.IsToUpdate) // para actualizar 
            {
                var updateAverage = (recipe.AverageRating * recipe.RatingCount + message.Rating) / recipe.RatingCount;
                recipe.AverageRating = updateAverage;

                await _recipeRepository.UpdateRecipeOnly(recipe); 
            }
            else // para crear 
            {
                var newAverage = (recipe.AverageRating * recipe.RatingCount + message.Rating) / (recipe.RatingCount + 1);
                recipe.RatingCount += 1;
                recipe.AverageRating = newAverage;

                await _recipeRepository.UpdateRecipeOnly(recipe);
            }

            await Task.CompletedTask; 
        }
    }
}