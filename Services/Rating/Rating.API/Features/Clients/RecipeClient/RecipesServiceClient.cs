using InvalidOperationException = BuildingBlocks.Exceptions.InvalidOperationException;

namespace Rating.API.Features.Clients.RecipeClient
{
    public class RecipesServiceClient : IRecipesServiceClient
    {
        private readonly HttpClient _httpClient;

        public RecipesServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RecipeExistAsync(int recipeId, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = $"api/recipes/{recipeId}";

                var response = await _httpClient.GetAsync(endpoint, cancellationToken); 

                // si retorna un 200  
                if (response.IsSuccessStatusCode)
                {
                    return true; 
                }

                // si retornar un 404 
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false; 
                }

                throw new 
                    InvalidOperationException("Respuesta inesperada de recipeService"); 
            }
            catch (HttpRequestException ex)
            {
                throw new 
                    InvalidOperationException("No se puede comunicar con RecipeService. Es posible que el servicio no esté disponible.");
            }
            catch (Exception ex)
            {
                throw; 
            }
        }
    }
}
