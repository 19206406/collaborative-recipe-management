using Recipe.API.Common.Database;
using Recipe.API.Entities;
using Recipe.API.Repositories.RepositoryInterfaces;

namespace Recipe.API.Repositories
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
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveIngredients(List<Ingredient> items)
        {
            _context.Ingredients.RemoveRange(items);
            return await _context.SaveChangesAsync() > 0; 
        }

        public async Task<bool> UpdateIngredients(List<Ingredient> items)
        {
            _context.Ingredients.UpdateRange(items);
            return await _context.SaveChangesAsync() > 0; 
        }
    }
}
