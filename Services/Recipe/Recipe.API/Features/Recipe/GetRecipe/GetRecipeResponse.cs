using Recipe.API.Features.Recipe.CreateRecipe;

namespace Recipe.API.Features.Recipe.GetRecipe
{
    public record GetRecipeResponse(ResponseRecipe recipe, ResponseIngredient Ingredient, 
        ResponseStep Step, ResponseTag Tag); 
}
