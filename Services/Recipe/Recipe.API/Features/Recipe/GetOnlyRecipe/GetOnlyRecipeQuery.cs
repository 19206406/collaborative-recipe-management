using BuildingBlocks.CQRS;

namespace Recipe.API.Features.Recipe.GetOnlyRecipe
{
    public record GetOnlyRecipeQuery(int Id) : IQuery<GetOnlyRecipeResponse>;
}
