using Microsoft.EntityFrameworkCore;
using Recipe.API.Common.Database;
using Recipe.API.Models;

namespace Recipe.API.Repositories.RecipeRepository
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

        public async Task<Entities.Recipe?> GetRecipe(int id)
        {
            var recipe = await _context.Recipes
                .AsSplitQuery() // dividir la soconsulta en varios selects y no en unico Join 
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .Include(r => r.RecipeTags)
                .FirstOrDefaultAsync(r => r.Id == id);

            return recipe;  
        }

        private IQueryable<Entities.Recipe> BuildRecipeQuery(RecipeSearchCriteria criteria)
        {
            IQueryable<Entities.Recipe> query = _context.Recipes
                .Include(r => r.RecipeTags);

            if (!string.IsNullOrWhiteSpace(criteria.Title))
                query = query.Where(r => r.Title.Contains(criteria.Title));

            if (criteria.PrepTimeMinutes.HasValue)
                query = query.Where(r => r.PrepTimeMinutes >= criteria.PrepTimeMinutes.Value);

            if (criteria.CookTimeMinutes.HasValue)
                query = query.Where(r => r.CookTimeMinutes >= criteria.CookTimeMinutes.Value);

            if (criteria.Servings.HasValue)
                query = query.Where(r => r.Servings >= criteria.Servings.Value);

            if (!string.IsNullOrWhiteSpace(criteria.Difficulty))
                query = query.Where(r => r.Difficulty.ToLower() == criteria.Difficulty);

            if (criteria.Tags is not null && criteria.Tags.Any())
                query = query.Where(r =>
                    r.RecipeTags.Any(rt => criteria.Tags.Contains(rt.Tag)));

            query = criteria.SortBy?.ToLower() switch
            {
                "title" => criteria.SortDescending
                    ? query.OrderByDescending(r => r.Title)
                    : query.OrderBy(r => r.Title),

                "preptime" => criteria.SortDescending
                    ? query.OrderByDescending(r => r.PrepTimeMinutes)
                    : query.OrderBy(r => r.PrepTimeMinutes),

                "difficulty" => criteria.SortDescending
                    ? query.OrderByDescending(r => r.Difficulty)
                    : query.OrderBy(r => r.Difficulty),

                "createdat" => criteria.SortDescending
                    ? query.OrderByDescending(r => r.CreatedAt)
                    : query.OrderBy(r => r.CreatedAt),

                _ => query.OrderBy(r => r.Id)
            };

            return query; 
        }

        public async Task<List<Entities.Recipe>> GetRecipePagination(int pageNumber, int pageSize, RecipeSearchCriteria criteria)
        {
            var query = BuildRecipeQuery(criteria);

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(); 
        }

        public async Task<List<Entities.Recipe>> SearchAdvanced(RecipeSearchCriteria criteria)
        {
            var query = BuildRecipeQuery(criteria);

            return await query
                .Take(20)
                .ToListAsync(); 
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

        public async Task RemoveRecipe(Entities.Recipe recipe)
        {
            _context.Recipes.Remove(recipe); 
            await _context.SaveChangesAsync();
        }

        public Task UpdateRecipeOnly(Entities.Recipe recipe)
        {
            // causante del tracker 
            if (_context.Entry(recipe).State == EntityState.Detached)
                _context.Entry(recipe).State = EntityState.Modified;

            // la receta fue llamada con el mismo dbContext ya se dectecta como modificada 
            // solo es guardar cambios con SaveChangesAsync(); 

            return Task.CompletedTask; 
        }

        public async Task<List<Entities.Recipe>> GetRecipesByIngredientsAsync(List<string> ingredients)
        {
            var count = ingredients.Count;

            return await _context.Recipes
                .Where(r =>
                    r.Ingredients
                        .Where(i => ingredients.Contains(i.Name))
                        .Select(i => i.Name)
                        .Distinct()
                        .Count() == count)
                .Include(r => r.Ingredients)
                .Include(r => r.RecipeTags)
                .ToListAsync();
        }

        public async Task<List<Entities.Recipe>> GetTopRecipesAsync()
        {

            var topRecipeIds = await _context.Recipes
                .Where(r => r.RatingCount != 0)
                .Where(r => r.CreatedAt < DateTime.UtcNow)
                .Select(r => new
                {
                    r.Id,
                    Score =
                        ((double)r.AverageRating * r.RatingCount) /
                        ((DateTime.UtcNow - r.CreatedAt).Days + 2.0)
                })
                .OrderByDescending(x => x.Score)
                .Take(10)
                .Select(x => x.Id)
                .ToListAsync();

            var recipes = await _context.Recipes
                .AsSplitQuery()
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .Include(r => r.RecipeTags)
                .Where(r => topRecipeIds.Contains(r.Id))
                .ToListAsync();

            return recipes;
        }

        public async Task<Entities.Recipe?> GetRecipeOnly(int id)
        {
            return await _context.Recipes.FirstOrDefaultAsync(r => r.Id == id); 
        }

        public async Task UpdateRatingRecipeOnly()
        {
            await _context.SaveChangesAsync(); 
        }
    }
}
