using BuildingBlocks.CQRS;

namespace Recipe.API.Features.Recipe.GetTopRecipes
{
    public record GetTopRecipesQuery() : IQuery<List<GetTopRecipesResponse>>; 
}
