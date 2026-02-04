using BuildingBlocks.CQRS;

namespace Recipe.API.Features.Recipe.UpdateRecipe
{
    public record UpdateRecipeCommand(UpdateRecipe Recipe, List<UpdateIngredient> Ingredients, List<UpdateStep> Steps) 
        : ICommand<UpdateRecipeResponse>; 
}
