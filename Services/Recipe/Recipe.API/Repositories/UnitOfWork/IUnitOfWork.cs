namespace Recipe.API.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken = default); 
    }
}
