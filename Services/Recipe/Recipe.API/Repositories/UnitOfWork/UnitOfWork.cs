using Recipe.API.Common.Database;

namespace Recipe.API.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RecipeDbContext _context;

        public UnitOfWork(RecipeDbContext context)
        {
            _context = context;
        }

        public Task CommitAsync(CancellationToken cancellationToken = default) 
            => _context.SaveChangesAsync(cancellationToken); 
    }
}
