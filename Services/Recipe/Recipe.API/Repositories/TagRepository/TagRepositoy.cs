using Microsoft.EntityFrameworkCore;
using Recipe.API.Common.Database;
using Recipe.API.Entities;

namespace Recipe.API.Repositories.TagRepository
{
    public class TagRepositoy : ITagRepository
    {
        private readonly RecipeDbContext _context;

        public TagRepositoy(RecipeDbContext context)
        {
            _context = context;
        }

        public async Task AddTagAsync(RecipeTag tag)
        {
            await _context.RecipeTags.AddAsync(tag); 
        }

        public Task DeleteTagAsync(int id)
        {
            var tracked = _context.ChangeTracker.Entries<RecipeTag>()
                .FirstOrDefault(e => e.Entity.Id == id);

            if (tracked is not null)
                _context.RecipeTags.Remove(tracked.Entity); 
            else
            {
                var stud = new RecipeTag { Id = id };
                _context.RecipeTags.Attach(stud);
                _context.RecipeTags.Remove(stud); 
            }

            return Task.CompletedTask; 
        }

        public async Task<List<RecipeTag>> GetTagsByIdRecipeAsync(int recipeId)
        {
            return await _context.RecipeTags
                .AsNoTracking()
                .Where(rt => rt.RecipeId == recipeId)
                .ToListAsync(); 
        }

        public Task UpdateTagAsync(RecipeTag tag)
        {
            if (_context.Entry(tag).State == EntityState.Detached)
                _context.Entry(tag).State = EntityState.Modified;

            return Task.CompletedTask; 
        }
    }
}
