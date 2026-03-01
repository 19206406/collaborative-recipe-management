using Recipe.API.Common.Database;
using Recipe.API.Entities;

namespace Recipe.API.Repositories.IngredientRepository
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly RecipeDbContext _context;

        public IngredientRepository(RecipeDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddIngredients(List<Ingredient> items)
        {
            _context.Ingredients.AddRange(items);
            await _context.SaveChangesAsync();
            return true; 
        }

        public async Task<bool> RemoveIngredients(List<Ingredient> items)
        {
            _context.Ingredients.RemoveRange(items);
            await _context.SaveChangesAsync();
            return true; 
        }

        public async Task<bool> UpdateIngredients(List<Ingredient> items)
        {
            _context.Ingredients.UpdateRange(items);
            await _context.SaveChangesAsync();
            return true; 
        }
    }
}
