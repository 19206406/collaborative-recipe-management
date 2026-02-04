using Recipe.API.Features.Recipe.CreateRecipe;

namespace Recipe.API.Features.Recipe.UpdateRecipe
{
    public record UpdateRecipeResponse(ResponseRecipe recipe, List<ResponseIngredient> Ingredient,
        List<ResponseStep> Step, List<ResponseTag> Tag); 
}
