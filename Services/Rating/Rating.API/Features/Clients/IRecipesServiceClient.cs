namespace Rating.API.Features.Clients
{
    public interface IRecipesServiceClient
    {
        Task<bool> RecipeExistAsync(int recipeId, CancellationToken cancellationToken = default);
    }
}
