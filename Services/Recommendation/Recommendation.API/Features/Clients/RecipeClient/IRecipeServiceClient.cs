using Recommendation.API.Common.Dtos;

namespace Recommendation.API.Features.Clients.RecipeClient
{
    public interface IRecipeServiceClient
    {
        Task<List<RecipeDto>> GetByIngredientsAsync(List<string> ingredients);
        Task<List<RecipeDto>> GetTopRecipes(CancellationToken cancellation = default);
        Task<List<RecipeDto>> GetPersonalizedRecipesAsync(List<string> tags, CancellationToken cancellationToken = default); 
    }
}
