using BuildingBlocks.CQRS;

namespace Rating.API.Features.Rating.GetAverageRating
{
    public record GetAverageRatingQuery(int RecipeId) : IQuery<GetAverageRatingResponse>; 
}
