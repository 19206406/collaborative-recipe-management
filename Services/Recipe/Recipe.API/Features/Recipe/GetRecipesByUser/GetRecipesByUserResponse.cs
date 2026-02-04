using Recipe.API.Features.Recipe.CreateRecipe;

namespace Recipe.API.Features.Recipe.GetRecipesByUser
{
    public record GetRecipesByUserResponse(List<ResponseRecipe> Recipes); 
}
