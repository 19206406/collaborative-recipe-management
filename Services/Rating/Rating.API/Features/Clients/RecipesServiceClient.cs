using InvalidOperationException = BuildingBlocks.Exceptions.InvalidOperationException;

namespace Rating.API.Features.Clients
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
                    var content = await response.Content.ReadAsStringAsync(cancellationToken);
                    return true; 
                }

                // si retornar un 404 
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var content = await response.Content.ReadAsStringAsync(cancellationToken);
                    return false; 
                }

                throw new 
                    InvalidOperationException("Response inesperado de recipeService"); 
            }
            catch (HttpRequestException ex)
            {
                throw new 
                    InvalidOperationException("No se puede comunicar con RecipesService. Es posible que el servicio no esté disponible.");
            }
            catch (Exception ex)
            {
                throw; 
            }
        }
    }
}
