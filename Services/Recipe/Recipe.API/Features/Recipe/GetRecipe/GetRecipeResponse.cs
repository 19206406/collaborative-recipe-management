using Recipe.API.Features.Recipe.CreateRecipe;

namespace Recipe.API.Features.Recipe.GetRecipe
{
    public record GetRecipeResponse(ResponseRecipe recipe, List<ResponseIngredient> Ingredients, 
        List<ResponseStep> Steps, List<ResponseTag> Tags); 
}
