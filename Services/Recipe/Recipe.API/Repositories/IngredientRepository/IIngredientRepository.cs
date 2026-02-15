using Recipe.API.Entities;

namespace Recipe.API.Repositories.IngredientRepository
{
    public interface IIngredientRepository
    {
        Task<bool> RemoveIngredients(List<Ingredient> items);
        Task<bool> UpdateIngredients(List<Ingredient> items);
        Task<bool> AddIngredients(List<Ingredient> items); 
    }
}
