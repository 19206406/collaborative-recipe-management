using BuildingBlocks.CQRS;

namespace Rating.API.Features.Rating.GetAEspecificRating
{
    public record GetAEspecificRatingQuery(int UserId, int RecipeId) : IQuery<GetAEspecificRatingResponse>; 
}
