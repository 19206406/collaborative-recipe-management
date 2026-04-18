using Mapster;
using Notification.API.Common.Dtos;
using InvalidOperationException = BuildingBlocks.Exceptions.InvalidOperationException;

namespace Notification.API.Features.Clients.UserClient
{
    public class UserServiceClient : IUserServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserServiceClient> _logger;

        public UserServiceClient(HttpClient httpClient, ILogger<UserServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<UserDto> UserRecipeRatingAsync(int userId)
        {
            _logger.LogInformation($"Recuperando usuario {userId} de userService"); 

            try
            {
                var endpoint = $"api/users/{userId}/basic";

                var response = await _httpClient.GetAsync(endpoint);

                var user = response.Adapt<UserDto>();

                if (user is null)
                    _logger.LogWarning("UserService ha devuelto un resultado satisfactorio pero el cuerpo del mensaje está" +
                        "vacío para el UserId {userId}", userId);

                _logger.LogInformation("Usuario {userId} recuperada correctamente", userId);

                return user; 
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"Respuesta inesperada de userService: {ex}"); 
            } 
            catch (Exception ex)
            {
                throw; 
            } 
        }
    }
}
