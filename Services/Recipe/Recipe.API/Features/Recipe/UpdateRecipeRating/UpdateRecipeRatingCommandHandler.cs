using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Recipe.API.Repositories.RecipeRepository;
using Recipe.API.Repositories.UnitOfWork;

namespace Recipe.API.Features.Recipe.UpdateRecipeRating
{
    public class UpdateRecipeRatingCommandHandler : ICommandHandler<UpdateRecipeRatingCommand, UpdateRecipeRatingResponse>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRecipeRatingCommandHandler(IRecipeRepository recipeRepository, IUnitOfWork unitOfWork)
        {
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateRecipeRatingResponse> Handle(UpdateRecipeRatingCommand command, CancellationToken cancellationToken)
        {
            var recipe = await _recipeRepository.GetRecipe(command.Id);

            if (recipe is null)
                throw new NotFoundException("receta", command.Id);

            // actualizar
            // TODO: verificar con el otro servicio si esta es la actualización correcta 
            recipe.AverageRating = command.NewAverage;
            recipe.RatingCount = command.NewRatingCount; 

            await _recipeRepository.UpdateRecipeOnly(recipe);
            await _unitOfWork.CommitAsync(cancellationToken); 

            return new UpdateRecipeRatingResponse(recipe.Id, recipe.UserId, recipe.Title, recipe.Description,
                recipe.PrepTimeMinutes, recipe.CookTimeMinutes, recipe.Difficulty, recipe.Servings, recipe.ImageUrl,
                recipe.AverageRating, recipe.RatingCount, recipe.UpdatedAt); 
        }
    }
}
