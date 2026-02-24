using BuildingBlocks.CQRS;

namespace Recipe.API.Features.Recipe.GetListOfRecipes
{
    public record GetListOfRecipesQuery(int PageNumber, int PageSize, SearchAdvancedRecipe Criteria) : IQuery<GetListOfRecipesResponse>;  
}
