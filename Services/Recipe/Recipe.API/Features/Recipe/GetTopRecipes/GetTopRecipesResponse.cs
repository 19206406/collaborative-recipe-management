using Recipe.API.Features.Recipe.CreateRecipe;

namespace Recipe.API.Features.Recipe.GetTopRecipes
{
    public record GetTopRecipesResponse(List<ResponseRecipe> Recipes); 
}
