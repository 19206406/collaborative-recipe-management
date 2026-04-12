using BuildingBlocks.CQRS;

namespace Recipe.API.Features.Recipe.GetRecipesByIngredients
{
    public record GetRecipesByIngredientsQuery(List<string> Ingredients) : IQuery<List<GetRecipesByIngredientsResponse>>;  
}
