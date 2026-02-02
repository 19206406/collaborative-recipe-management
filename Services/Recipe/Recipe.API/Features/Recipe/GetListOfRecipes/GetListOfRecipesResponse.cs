using BuildingBlocks.Pagination;
using Recipe.API.Features.Recipe.CreateRecipe;

namespace Recipe.API.Features.Recipe.GetListOfRecipes
{
    public record GetListOfRecipesResponse(PaginatedResult<ResponseRecipe> Recipes); 
}
