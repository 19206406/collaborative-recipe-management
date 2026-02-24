using Recommendation.API.Common.Dtos;

namespace Recommendation.API.Features.Clients.RecipeClient
{
    public interface IRecipeServiceClient
    {
        Task<List<RecipeDto>> GetByIngredientsAsync(List<string> ingredients);
        Task<List<RecipeDto>> GetTopRecipes(CancellationToken cancellation = default);
        Task<List<RecipeDto>> GetPersonalizedRecipesAsync(List<string> tags, CancellationToken cancellationToken = default);
        Task<List<RecipeDto>> GetSimilarRecipesAsync(int recipeId, CancellationToken cancellationToken = default);
        Task<RecipeDto> GetRecipeAsync(int recipeId, CancellationToken cancellationToken); 
    }
}
