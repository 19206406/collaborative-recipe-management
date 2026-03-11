using Recipe.API.Entities;

namespace Recipe.API.Repositories.IngredientRepository
{
    public interface IIngredientRepository
    {
        Task AddIngredientAsync(Ingredient ingredient);
        Task UpdateIngredientAsync(Ingredient ingredient);
        Task DeleteIngredientAsync(int id); 
        Task<List<Ingredient>> GetIngredientsByIdRecipeAsync(int id); 
        Task<bool> RemoveIngredients(List<Ingredient> items);
        Task<bool> UpdateIngredients(List<Ingredient> items);
        Task<bool> AddIngredients(List<Ingredient> items); 
    }
}
