namespace Recipe.API.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        // TODO: Registrar en la clase program 
        Task CommitAsync(CancellationToken cancellationToken = default); 
    }
}
