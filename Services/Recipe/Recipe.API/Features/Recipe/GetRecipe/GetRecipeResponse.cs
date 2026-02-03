using Recipe.API.Features.Recipe.CreateRecipe;

namespace Recipe.API.Features.Recipe.GetRecipe
{
    public record GetRecipeResponse(ResponseRecipe recipe, List<ResponseIngredient> Ingredient, 
        List<ResponseStep> Step, List<ResponseTag> Tag); 
}
