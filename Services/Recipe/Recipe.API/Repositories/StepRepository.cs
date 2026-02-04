using Recipe.API.Common.Database;
using Recipe.API.Entities;

namespace Recipe.API.Repositories
{
    public class StepRepository : IStepRepository
    {
        private readonly RecipeDbContext _context;

        public StepRepository(RecipeDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddSteps(List<Step> items)
        {
            _context.Steps.AddRange(items);
            return await _context.SaveChangesAsync() > 0; 
        }

        public async Task<bool> RemoveSteps(List<Step> items)
        {
            _context.Steps.RemoveRange(items); 
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateSteps(List<Step> items)
        {
            _context.Steps.UpdateRange(items);
            return await _context.SaveChangesAsync() > 0; 
        }
    }
}
