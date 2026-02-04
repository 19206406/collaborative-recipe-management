using Microsoft.EntityFrameworkCore;
using Recipe.API.Common.Database;

namespace Recipe.API.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly RecipeDbContext _context;

        public RecipeRepository(RecipeDbContext context)
        {
            _context = context;
        }

        public async Task<Entities.Recipe> AddRecipe(Entities.Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return recipe; 
        }

        public Task<Entities.Recipe> GetRecipe(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Entities.Recipe>> GetRecipePagination(int pageNumber, int pageSize)
        {
            var recipes = await _context.Recipes
                .OrderBy(x => x.Difficulty)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return recipes; 
        }

        public async Task<List<Entities.Recipe>> GetRecipesByUser(int userId)
        {
            var recipes = await _context.Recipes
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return recipes; 
        }

        public async Task<long> NumberOfItems()
        {
            return await _context.Recipes.LongCountAsync(); 
        }

        public Task RemoveRecipe(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateRecipeOnly(Entities.Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();

            return true; 
        }
    }
}
