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

        public async Task<Entities.Recipe> GetRecipe(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .Include(r => r.RecipeTags)
                .FirstOrDefaultAsync(r => r.Id == id);

            return recipe;  
        }

        public async Task<List<Entities.Recipe>> GetRecipePagination(int pageNumber, int pageSize, RecipeSearchCriteria criteria)
        {
            IQueryable<Entities.Recipe> query = _context.Recipes.Include(r => r.Title);

            // aplicar filtros 

            if (!string.IsNullOrEmpty(criteria.Title))
                query = query.Where(r => r.Title.Contains(criteria.Title));

            if (criteria.PrepTimeMinutes.HasValue)
                query = query.Where(r => r.PrepTimeMinutes >= criteria.PrepTimeMinutes.Value);

            if (criteria.CookTimeMinutes.HasValue)
                query = query.Where(r => r.CookTimeMinutes >= criteria.CookTimeMinutes.Value);

            if (criteria.Difficulty.HasValue)
                query = query.Where(r => r.Difficulty == criteria.Difficulty.Value);

            // "Title", "Difficulty", "PrepTimeMinutes"
            if (!string.IsNullOrEmpty(criteria.SortBy))
            {
                query = criteria.SortBy.ToLower() switch
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

                    _ => query.OrderBy(r => r.Id)
                };
            }

            var recipes = await query
                .OrderBy(x => x.Difficulty)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return recipes; 
        }

        public async Task<List<Entities.Recipe>> SearchAdvanced(RecipeSearchCriteria criteria)
        {
            IQueryable<Entities.Recipe> query = _context.Recipes.Include(r => r.Title);

            // luego de esto aplicamos los filtros si se pasaron 
            // cada if agrega una condición al query 

            if (!string.IsNullOrEmpty(criteria.Title))
                query = query.Where(r => r.Title.Contains(criteria.Title));

            if (criteria.PrepTimeMinutes.HasValue)
                query = query.Where(r => r.PrepTimeMinutes >= criteria.PrepTimeMinutes.Value);

            if (criteria.CookTimeMinutes.HasValue)
                query = query.Where(r => r.CookTimeMinutes >= criteria.CookTimeMinutes.Value);

            if (criteria.Difficulty.HasValue)
                query = query.Where(r => r.Difficulty == criteria.Difficulty.Value);

            // "Title", "Difficulty", "PrepTimeMinutes"
            if (!string.IsNullOrEmpty(criteria.SortBy))
            {
                query = criteria.SortBy.ToLower() switch
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

                    _ => query.OrderBy(r => r.Id)
                };
            }

            // solo los primeros 20 ya que luego toca que implementar esto en el paginado
            var recipes = await query
                .Take(20)
                .ToListAsync();

            // ejecutar la consulta 
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

        public async Task RemoveRecipe(Entities.Recipe recipe)
        {
            _context.Recipes.Remove(recipe); 
            await _context.SaveChangesAsync();
        }

        public async Task<Entities.Recipe> UpdateRecipeOnly(Entities.Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();

            await _context.Entry(recipe).ReloadAsync();
            return recipe;
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
                .ToListAsync();
        }

        public async Task<List<Entities.Recipe>> GetTopRecipesAsync()
        {
            var trending = await _context.Recipes
                .Select(r => new {
                    Recipe = r,
                    Score = (r.AverageRating * r.RatingCount) /
                            (((DateTime.UtcNow - r.CreatedAt).Days) + 2m)
                })
                .OrderByDescending(x => x.Score)
                .Take(20)
                .Select(x => x.Recipe)
                .ToListAsync();

            return trending; 
        }
    }
}
