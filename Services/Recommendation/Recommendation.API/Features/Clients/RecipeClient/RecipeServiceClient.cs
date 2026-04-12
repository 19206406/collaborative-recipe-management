using Mapster;
using Recommendation.API.Common.Dtos;
using System.Web;

namespace Recommendation.API.Features.Clients.RecipeClient
{
    public class RecipeServiceClient : IRecipeServiceClient
    {
        private readonly HttpClient _httpClient;

        public RecipeServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RecipeDto>> GetByIngredientsAsync(List<string> ingredients)
        {
            try
            {
                var query = HttpUtility.ParseQueryString(string.Empty);
                foreach (var ingredient in ingredients)
                    query.Add("ingredients", ingredient);

                var result = await _httpClient.GetFromJsonAsync<List<RecipeDto>>(
                    $"/api/recipes/by-ingredients?{query}");

                return result ?? [];
            } 
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException("No se puede comunicar con recipeService. Es posible que el servicio no esté disponible."); 
            }
            catch (Exception ex)
            {
                throw; 
            }
        }

        public async Task<List<RecipeDto>> GetPersonalizedRecipesAsync(List<string>? tags, CancellationToken cancellationToken)
        {
            try
            {
                var query = HttpUtility.ParseQueryString(string.Empty);
                
                if (tags is not null)
                {
                    foreach (var tag in tags)
                        query.Add("tags", tag);
                }

                var endpoint = $"api/recipes/search?{query}";
                var recipes = await _httpClient.GetAsync(endpoint, cancellationToken);

                return recipes.Adapt<List<RecipeDto>>(); 
            }
            catch (HttpRequestException ex)
            {
                throw new
                    InvalidOperationException("No se puede comunicar con recipeService. Es posible que el servicio no esté disponible.");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<RecipeDto> GetRecipeAsync(int recipeId, CancellationToken cancellationToken)
        {
            try
            {
                var endpoint = $"api/recipes/get-only-recipe/{recipeId}";
                var recipe = await _httpClient.GetAsync(endpoint, cancellationToken);

                return recipe.Adapt<RecipeDto>(); 
            }
            catch (HttpRequestException ex)
            {
                throw new
                    InvalidOperationException("No se puede comunicar con recipeService. Es posible que el servicio no esté disponible.");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<RecipeDto>> GetSimilarRecipesAsync(int recipeId, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = $"api/recommendations/similar/{recipeId}";
                var recipes = await _httpClient.GetAsync(endpoint, cancellationToken);

                return recipes.Adapt<List<RecipeDto>>();    
            }
            catch (HttpRequestException ex)
            {
                throw new
                    InvalidOperationException("No se puede comunicar con recipeService. Es posible que el servicio no esté disponible.");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<RecipeDto>> GetTopRecipes(CancellationToken cancellation)
        {
            try
            {
                var endpoint = $"api/recipes/trending";
                var recipes = await _httpClient.GetAsync(endpoint, cancellation);

                return recipes.Adapt<List<RecipeDto>>(); 
            }
            catch (HttpRequestException ex)
            {
                throw new
                    InvalidOperationException("No se puede comunicar con recipeService. Es posible que el servicio no esté disponible.");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
