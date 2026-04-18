using Notification.API.Common.Dtos;

namespace Notification.API.Features.Clients.RecipeClient
{
    public interface IRecipeServiceClient
    {
        Task<RecipeDto> RecipeByIdAsync(int recipeId); 
    }
}
