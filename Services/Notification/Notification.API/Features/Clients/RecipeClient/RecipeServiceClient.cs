using Mapster;
using Notification.API.Common.Dtos;
using InvalidOperationException = BuildingBlocks.Exceptions.InvalidOperationException;

namespace Notification.API.Features.Clients.RecipeClient
{
    public class RecipeServiceClient : IRecipeServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RecipeServiceClient> _logger;

        public RecipeServiceClient(HttpClient httpClient, ILogger<RecipeServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<RecipeDto> RecipeByIdAsync(int recipeId)
        {

            _logger.LogInformation($"Recuperando la receta {recipeId} de recipeService"); 

            try
            {
                var endpoint = $"api/recipes/{recipeId}";

                var response = await _httpClient.GetFromJsonAsync<RecipeDto>(endpoint);

                if (response is null)
                    _logger.LogWarning("RecipeService ha devuelto un resultado satisfactorio, pero el cuerpo del mensaje está " +
                        "vacío para el RecipeId {recipeId}", recipeId);

                _logger.LogInformation("Receta {recipeId} recuperada correctamente", recipeId);

                return response; 
            } 
            catch (HttpRequestException ex)
            {
                throw new
                    InvalidOperationException("Respuesta inesperada de recipeService"); 
            } 
            catch (Exception ex)
            {
                throw; 
            }
        }
    }
}
