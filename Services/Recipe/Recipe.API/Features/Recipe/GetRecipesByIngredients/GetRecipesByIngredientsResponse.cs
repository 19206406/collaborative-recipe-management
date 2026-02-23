using Recipe.API.Features.Recipe.CreateRecipe;

namespace Recipe.API.Features.Recipe.GetRecipesByIngredients
{
    public record GetRecipesByIngredientsResponse(List<ResponseRecipe> Recipes); 
}
