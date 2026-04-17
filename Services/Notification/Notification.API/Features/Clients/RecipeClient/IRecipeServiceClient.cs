namespace Notification.API.Features.Clients.RecipeClient
{
    public interface IRecipeServiceClient
    {
        Task RecipeById(int recipeId); 
    }
}
