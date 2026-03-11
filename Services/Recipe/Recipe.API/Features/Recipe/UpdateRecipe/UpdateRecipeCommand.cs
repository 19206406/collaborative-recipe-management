using BuildingBlocks.CQRS;

namespace Recipe.API.Features.Recipe.UpdateRecipe
{
    public record UpdateRecipeCommand(
        int Id, 
        int UserId, 
        UpdateRecipe Recipe, 
        List<UpdateIngredient> Ingredients, 
        List<UpdateStep> Steps, 
        List<UpdateTag> Tags) 
        : ICommand<UpdateRecipeResponse>; 
}
