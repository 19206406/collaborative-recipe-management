using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Mapster;
using Recipe.API.Features.Recipe.CreateRecipe;
using Recipe.API.Repositories.RecipeRepository;

namespace Recipe.API.Features.Recipe.UpdateRecipeRating
{
    public class UpdateRecipeRatingCommandHandler : ICommandHandler<UpdateRecipeRatingCommand, UpdateRecipeRatingResponse>
    {
        private readonly IRecipeRepository _recipeRepository;

        public UpdateRecipeRatingCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<UpdateRecipeRatingResponse> Handle(UpdateRecipeRatingCommand command, CancellationToken cancellationToken)
        {
            var recipe = await _recipeRepository.GetRecipe(command.Id);

            if (recipe is null)
                throw new NotFoundException("receta", command.Id);

            // actualizar
            var updateAverage = (recipe.AverageRating * recipe.RatingCount + command.Rating) / recipe.RatingCount;
            recipe.AverageRating = updateAverage;

            var updatedRecipe = await _recipeRepository.UpdateRecipeOnly(recipe);

            return new UpdateRecipeRatingResponse(updatedRecipe.Adapt<ResponseRecipe>()); 
        }
    }
}
