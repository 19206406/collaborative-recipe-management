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

        public async Task<List<Entities.Recipe>> GetRecipePagination(int pageNumber, int pageSize, RecipeSearchCriteria criteria)
        {
            IQueryable<Entities.Recipe> query = _context.Recipes;
                //.Include(r => r.Ingredients)
                //.Include(r => r.Steps)
                //.Include(r => r.RecipeTags); 

            // aplicar filtros 

            if (!string.IsNullOrEmpty(criteria.Title))
                query = query.Where(r => r.Title.Contains(criteria.Title));

            if (criteria.PrepTimeMinutes.HasValue)
                query = query.Where(r => r.PrepTimeMinutes >= criteria.PrepTimeMinutes.Value);

            if (criteria.CookTimeMinutes.HasValue)
                query = query.Where(r => r.CookTimeMinutes >= criteria.CookTimeMinutes.Value);

            if (!string.IsNullOrEmpty(criteria.Difficulty))
                query = query.Where(r => r.Difficulty == criteria.Difficulty);

            // filtrar por tags 
            if (criteria.Tags is not null && criteria.Tags.Any())
                query = query.Where(r => r.RecipeTags.Any(rt => criteria.Tags.Contains(rt.Tag))); 

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

                    "CreatedAt" => criteria.SortDescending
                        ? query.OrderByDescending(r => r.CreatedAt)
                        : query.OrderBy(r => r.CreatedAt),

                    _ => query.OrderBy(r => r.Id)
                };
            }

            var recipes = await query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return recipes; 
        }

        public async Task<List<Entities.Recipe>> SearchAdvanced(RecipeSearchCriteria criteria)
        {
            IQueryable<Entities.Recipe> query = _context.Recipes.Include(r => r.RecipeTags);

            // luego de esto aplicamos los filtros si se pasaron 
            // cada if agrega una condición al query 

            if (!string.IsNullOrEmpty(criteria.Title))
                query = query.Where(r => r.Title.Contains(criteria.Title));

            if (criteria.PrepTimeMinutes.HasValue)
                query = query.Where(r => r.PrepTimeMinutes >= criteria.PrepTimeMinutes.Value);

            if (criteria.CookTimeMinutes.HasValue)
                query = query.Where(r => r.CookTimeMinutes >= criteria.CookTimeMinutes.Value);

            if (criteria.Difficulty != "")
                query = query.Where(r => r.Difficulty == criteria.Difficulty);

            // TODO: debo de agregar ordenamiento por rating y novedad 

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
            var recipes = await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .Include(r => r.RecipeTags)
                //.Where(r => r.CreatedAt < DateTime.UtcNow && r.RatingCount != 0 && r.RatingCount != 0) 
                .Where(r => r.CreatedAt < DateTime.UtcNow)
                .ToListAsync();

            var trending = recipes
                .Select(r => new
                {
                    Recipe = r,
                    Score = (r.AverageRating * r.RatingCount) /
                            ((decimal)(DateTime.UtcNow - r.CreatedAt).Days + 2m)
                })
                .OrderByDescending(x => x.Score)
                .Take(20)
                .Select(x => x.Recipe)
                .ToList();

            return trending;
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
