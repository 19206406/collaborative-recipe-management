using Rating.API.Features.Rating.GetRecipeRatings;

namespace Rating.API.Features.Rating.GetUserRatings
{
    public record GetUserRatingsResponse(List<RatingResponse> Ratings); 
}
