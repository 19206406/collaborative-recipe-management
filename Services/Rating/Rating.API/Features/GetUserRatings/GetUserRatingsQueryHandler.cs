using BuildingBlocks.CQRS;
using Mapster;
using Rating.API.Features.GetRecipeRatings;
using Rating.API.Repositories;

namespace Rating.API.Features.GetUserRatings
{
    public class GetUserRatingsQueryHandler : IQueryHandler<GetUserRatingsQuery, GetUserRatingsResponse>
    {
        private readonly IRatingRepository _ratingRepository;

        public GetUserRatingsQueryHandler(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<GetUserRatingsResponse> Handle(GetUserRatingsQuery query, CancellationToken cancellationToken)
        {
            var ratings = await _ratingRepository.GetRatingsByUserIdAsync(query.UserId);

            var mapRatings = ratings.Adapt<List<RatingResponse>>();

            return new GetUserRatingsResponse(mapRatings); 
        }
    }
}
