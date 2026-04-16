using Mapster;
using Recommendation.API.Common.Dtos;

namespace Recommendation.API.Features.Clients.RatingClient
{
    // TODO: Debo de verificar si los endpoints de los otros servicios si me estan retornando lo que necesito en este servicio
    public class RatingServiceClient : IRatingServiceClient
    {
        private readonly HttpClient _httpClient;

        public RatingServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserRatingDto>> GetRatingsByUserAsync(int userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = $"/api/ratings/user/{userId}";

                var userRatings = await _httpClient.GetFromJsonAsync<List<UserRatingDto>>(endpoint, cancellationToken);

                return userRatings; 
            }
            catch (HttpRequestException ex)
            {
                throw new
                    InvalidOperationException("No se puede comunicar con ratingService. Es posible que el servicio no esté disponible.");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
