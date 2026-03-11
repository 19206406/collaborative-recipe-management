using Microsoft.EntityFrameworkCore;
using Recipe.API.Common.Database;
using Recipe.API.Entities;

namespace Recipe.API.Repositories.StepRepository
{
    public class StepRepository : IStepRepository
    {
        private readonly RecipeDbContext _context;

        public StepRepository(RecipeDbContext context)
        {
            _context = context;
        }

        public async Task<List<Step>> GetStepsByIdRecipeAsync(int recipeId)
        {
            return await _context.Steps
                .Where(x => x.RecipeId == recipeId)
                .OrderBy(x => x.StepNumber)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddStepAsync(Step step)
        {
            await _context.Steps.AddAsync(step);
        }

        public Task UpdateStepAsync(Step step)
        {
            if (_context.Entry(step).State == EntityState.Detached)
                _context.Entry(step).State = EntityState.Modified;

            return Task.CompletedTask; 
        }

        public Task DeleteStepAsync(int id)
        {
            var tracked = _context.ChangeTracker
                .Entries<Step>()
                .FirstOrDefault(s => s.Entity.Id == id);

            if (tracked is not null)
                _context.Steps.Remove(tracked.Entity); 
            else
            {
                var stup = new Step { Id = id };
                _context.Steps.Attach(stup);
                _context.Steps.Remove(stup); 
            }

            return Task.CompletedTask; 
        }

        public async Task<bool> RemoveSteps(List<Step> items)
        {
            if (!items.Any()) return true;

            _context.ChangeTracker.Clear();
            _context.Steps.RemoveRange(items);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
            return true;
        }

        public async Task<bool> AddSteps(List<Step> items, int lenSteps)
        {
            for (int i = 0; i < items.Count; i++)
                items[i].StepNumber = lenSteps + i + 1;
            _context.Steps.AddRange(items);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateSteps(List<Step> items)
        {
            if (!items.Any()) return true;

            foreach (var item in items)
            {
                var tracked = _context.ChangeTracker.Entries<Step>()
                    .FirstOrDefault(e => e.Entity.Id == item.Id);
                if (tracked is not null)
                    tracked.CurrentValues.SetValues(item);
                else
                {
                    _context.Entry(item).State = EntityState.Detached;
                    _context.Steps.Update(item);
                }
            }
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
            return true;
        }

        
    }
}
