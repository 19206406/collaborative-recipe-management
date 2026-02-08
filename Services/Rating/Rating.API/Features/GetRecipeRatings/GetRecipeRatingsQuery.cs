using BuildingBlocks.CQRS;

namespace Rating.API.Features.GetRecipeRatings
{
    public record GetRecipeRatingsQuery(int RecipeId) : IQuery<GetRecipeRatingsResponse>; 
}
