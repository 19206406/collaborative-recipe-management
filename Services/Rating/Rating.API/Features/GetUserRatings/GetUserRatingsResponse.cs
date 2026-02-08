using Rating.API.Features.GetRecipeRatings;

namespace Rating.API.Features.GetUserRatings
{
    public record GetUserRatingsResponse(List<RatingResponse> Ratings); 
}
