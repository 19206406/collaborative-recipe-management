using Mapster;
using Rating.API.Common.Dtos;
using InvalidOperationException = BuildingBlocks.Exceptions.InvalidOperationException;

namespace Rating.API.Features.Clients.NotificationClient
{
    public class NotificationServiceClient : INotificationServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NotificationServiceClient> _logger;

        public NotificationServiceClient(HttpClient httpClient, ILogger<NotificationServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> CreateNewNotificationAsync(CreateNotificationRequest request)
        {
            _logger.LogInformation("Creando una nueva notificación desde ratingService"); 

            try
            {
                var endpoint = "api/notifications/rating-received";

                var response = await _httpClient.PostAsJsonAsync(endpoint, request);

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return false; 

                if (response.Content is null)
                    _logger.LogWarning("NotificationService realizo una petición satisfactoria pero el cuerpo del mensaje está " +
                        "vacío para la creación de esta nueva notificación verifica los resultados en el servicio");

                _logger.LogInformation("Notificación de calificación creada correctamente");

                return true; 
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
