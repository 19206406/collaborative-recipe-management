using BuildingBlocks.CQRS;

namespace Rating.API.Features.Rating.GetRecipeRatings
{
    public record GetRecipeRatingsQuery(int RecipeId) : IQuery<GetRecipeRatingsResponse>; 
}
