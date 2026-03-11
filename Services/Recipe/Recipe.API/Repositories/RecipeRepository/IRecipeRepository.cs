using Recipe.API.Models;

namespace Recipe.API.Repositories.RecipeRepository
{
    public interface IRecipeRepository
    {
        Task<Entities.Recipe> AddRecipe(Entities.Recipe recipe);
        Task<Entities.Recipe?> GetRecipe(int id);
        Task<Entities.Recipe?> GetRecipeOnly(int id); 
        Task<List<Entities.Recipe>> GetRecipesByUser(int userId); 
        Task RemoveRecipe(Entities.Recipe recipe);
        Task<long> NumberOfItems();
        Task<List<Entities.Recipe>> GetRecipePagination(int pageNumber, int pageSize, RecipeSearchCriteria criteria);

        Task UpdateRecipeOnly(Entities.Recipe recipe);
        Task<List<Entities.Recipe>> SearchAdvanced(RecipeSearchCriteria criteria);

        Task<List<Entities.Recipe>> GetRecipesByIngredientsAsync(List<string> ingredients);

        Task<List<Entities.Recipe>> GetTopRecipesAsync(); 
    }
}
