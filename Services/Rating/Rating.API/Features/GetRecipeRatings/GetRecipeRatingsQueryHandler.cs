using BuildingBlocks.CQRS;
using Mapster;
using Rating.API.Repositories;

namespace Rating.API.Features.GetRecipeRatings
{
    public class GetRecipeRatingsQueryHandler : IQueryHandler<GetRecipeRatingsQuery, GetRecipeRatingsResponse>
    {
        private readonly IRatingRepository _ratingRepository;

        public GetRecipeRatingsQueryHandler(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<GetRecipeRatingsResponse> Handle(GetRecipeRatingsQuery query, CancellationToken cancellationToken)
        {
            var ratings = await _ratingRepository.GetRatingsByRecipeIdAsync(query.RecipeId);

            var ratingsMap = ratings.Adapt<List<RatingResponse>>();

            return new GetRecipeRatingsResponse(ratingsMap); 
        }
    }
}
