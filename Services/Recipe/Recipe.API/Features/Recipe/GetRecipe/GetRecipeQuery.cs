using BuildingBlocks.CQRS;

namespace Recipe.API.Features.Recipe.GetRecipe
{
    public record GetRecipeQuery(int Id) : IQuery<GetRecipeResponse>; 
}
