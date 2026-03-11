using Microsoft.EntityFrameworkCore;
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

        public Task<List<Ingredient>> GetIngredientsByIdRecipeAsync(int recipeId)
        {
            return _context.Ingredients
                .Where(x => x.RecipeId == recipeId)
                .AsNoTracking() // solo lectura para comparar -> sin overhead de tracking 
                .ToListAsync(); 
        }

        public async Task AddIngredientAsync(Ingredient ingredient)
        {
            await _context.Ingredients.AddAsync(ingredient);
        }

        public async Task<bool> AddIngredients(List<Ingredient> items)
        {
            _context.Ingredients.AddRange(items);
            await _context.SaveChangesAsync();
            return true; 
        }

        public async Task<bool> RemoveIngredients(List<Ingredient> items)
        {
            if (!items.Any()) return true;

            _context.ChangeTracker.Clear();
            _context.Ingredients.RemoveRange(items);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
            return true;
        }

        public async Task<bool> UpdateIngredients(List<Ingredient> items)
        {
            if (!items.Any()) return true;

            foreach (var item in items)
            {
                var tracked = _context.ChangeTracker.Entries<Ingredient>()
                    .FirstOrDefault(e => e.Entity.Id == item.Id);
                if (tracked is not null)
                    tracked.CurrentValues.SetValues(item);
                else
                {
                    _context.Entry(item).State = EntityState.Detached; // por si acaso
                    _context.Ingredients.Update(item);
                }
            }
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
            return true;
        }

        public Task UpdateIngredientAsync(Ingredient ingredient)
        {
            if (_context.Entry(ingredient).State == EntityState.Detached)
                _context.Entry(ingredient).State = EntityState.Modified;

            return Task.CompletedTask; 
        }

        public Task DeleteIngredientAsync(int id)
        {
            // verificar si se encuentra en el include 
            var tracked = _context.ChangeTracker.Entries<Ingredient>()
                .FirstOrDefault(e => e.Entity.Id == id);

            if (tracked is not null)
                _context.Ingredients.Remove(tracked.Entity); 
            else
            {
                var stub = new Ingredient { Id = id };
                _context.Ingredients.Attach(stub);
                _context.Ingredients.Remove(stub); 
            }

            return Task.CompletedTask;  
        }
    }
}
