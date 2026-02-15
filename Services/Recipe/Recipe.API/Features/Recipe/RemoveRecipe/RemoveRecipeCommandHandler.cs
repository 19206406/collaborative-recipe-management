using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using MediatR;
using Recipe.API.Repositories.RecipeRepository;

namespace Recipe.API.Features.Recipe.RemoveRecipe
{
    public class RemoveRecipeCommandHandler : ICommandHandler<RemoveRecipeCommand>
    {
        private readonly IRecipeRepository _recipeRepository;

        public RemoveRecipeCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<Unit> Handle(RemoveRecipeCommand command, CancellationToken cancellationToken)
        {
            var recipe = await _recipeRepository.GetRecipe(command.Id);

            if (recipe is null)
                throw new NotFoundException("receta", command.Id);

            if (recipe.UserId != command.UserId)
                throw new UnauthorizedException("Usuario no autorizado para ejecutar esta acción"); 

            await _recipeRepository.RemoveRecipe(recipe);

            return Unit.Value; 
        }
    }
}
