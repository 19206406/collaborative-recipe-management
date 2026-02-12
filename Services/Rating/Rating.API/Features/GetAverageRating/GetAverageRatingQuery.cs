using BuildingBlocks.CQRS;

namespace Rating.API.Features.GetAverageRating
{
    public record GetAverageRatingQuery(int RecipeId) : IQuery<GetAverageRatingResponse>; 
}
