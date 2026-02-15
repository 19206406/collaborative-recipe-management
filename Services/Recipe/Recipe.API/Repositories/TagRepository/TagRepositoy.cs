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

        public async Task<bool> UpdateTags(List<RecipeTag> items)
        {
            _context.UpdateRange(items);
            return await _context.SaveChangesAsync() > 0; 
        }
    }
}
