using BuildingBlocks.CQRS;

namespace Recipe.API.Features.Recipe.CreateRecipe
{
    public record CreateRecipeCommand(CreateRecipe Recipe, List<CreateIngredient> Ingredients, List<CreateStep> Steps) 
        : ICommand<CreateRecipeResponse>; 
}
