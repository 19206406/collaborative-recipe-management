using Recommendation.API.Common.Dtos;

namespace Recommendation.API.Features.Clients.RatingClient
{
    public interface IRatingServiceClient
    {
        Task<List<UserRatingDto>> GetRatingsByUserAsync(int userId, CancellationToken cancellationToken = default);  

    }
}
