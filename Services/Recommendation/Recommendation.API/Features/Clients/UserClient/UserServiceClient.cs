using Mapster;
using Recommendation.API.Common.Dtos;
using InvalidOperationException = BuildingBlocks.Exceptions.InvalidOperationException;

namespace Recommendation.API.Features.Clients.UserClient
{
    public class UserServiceClient : IUserServiceClient
    {
        private readonly HttpClient _httpClient;

        public UserServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserPreferencesDto> GetUserPreferencesAsync(int userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = $"api/users/{userId}/preferences";

                var preferences = await _httpClient.GetAsync(endpoint);

                return preferences.Adapt<UserPreferencesDto>(); 
            }
            catch (HttpRequestException ex)
            {
                throw new
                    InvalidOperationException("No se puede comunicar con UserService. Es posible que el servicio no esté disponible.");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UserExistByIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = $"api/users/{userId}";

                var response = await _httpClient.GetAsync(endpoint, cancellationToken);

                // si retorna un 200 
                if (response.IsSuccessStatusCode)
                    return true;

                // si retorna un 404 
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                throw new
                    InvalidOperationException("Respuesta inesperada de userService");


            }
            catch (HttpRequestException ex)
            {
                throw new
                    InvalidOperationException("No se puede comunicar con UserService. Es posible que el servicio no esté disponible.");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
