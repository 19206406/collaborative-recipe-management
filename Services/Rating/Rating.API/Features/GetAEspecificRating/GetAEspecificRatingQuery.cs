using BuildingBlocks.CQRS;

namespace Rating.API.Features.GetAEspecificRating
{
    public record GetAEspecificRatingQuery(int UserId, int RecipeId) : IQuery<GetAEspecificRatingResponse>; 
}
