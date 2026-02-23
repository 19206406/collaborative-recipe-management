using Recommendation.API.Common.Dtos;
using System.Web;

namespace Recommendation.API.Features.Clients.RecipeClient
{
    public class RecipeServiceClient(HttpClient httpClient) : IRecipeServiceClient
    {
        public async Task<List<RecipeDto>> GetByIngredientsAsync(List<string> ingredients)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var ingredient in ingredients)
                query.Add("ingredients", ingredient);

            var result = await httpClient.GetFromJsonAsync<List<RecipeDto>>(
                $"/api/recipes/by-ingredients?{query}");

            return result ?? []; 
        }

        public Task<List<RecipeDto>> GetTopRecipes()
        {
            try
            {
                var endpoint = $""; 
            }
            catch (Exception ex)
        }
    }
}
