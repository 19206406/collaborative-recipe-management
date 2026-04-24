namespace Rating.API.Features.Clients.RecipeClient
{
    public interface IRecipesServiceClient
    {
        Task<bool> RecipeExistAsync(int recipeId, CancellationToken cancellationToken = default);
    }
}
