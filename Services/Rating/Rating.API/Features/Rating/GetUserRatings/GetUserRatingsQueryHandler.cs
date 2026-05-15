using BuildingBlocks.CQRS;
using Mapster;
using Rating.API.Repositories;

namespace Rating.API.Features.Rating.GetUserRatings
{
    public class GetUserRatingsQueryHandler : IQueryHandler<GetUserRatingsQuery, List<GetUserRatingsResponse>>
    {
        private readonly IRatingRepository _ratingRepository;

        public GetUserRatingsQueryHandler(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<List<GetUserRatingsResponse>> Handle(GetUserRatingsQuery query, CancellationToken cancellationToken)
        {
            var ratings = await _ratingRepository.GetRatingsByUserIdAsync(query.UserId);

            var mapRatings = ratings.Adapt<List<GetUserRatingsResponse>>();

            return mapRatings; 
        }
    }
}
