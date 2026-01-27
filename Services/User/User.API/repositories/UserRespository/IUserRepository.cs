namespace User.API.repositories.UserRespository
{
    public interface IUserRepository
    {
        Task<int> AddUser(Entities.User entity);
        Task<Entities.User?> GetUser(int id);
        Task DeleteUser(Entities.User entity);
        Task UpdateUser(Entities.User entity);
        Task<Entities.User?> GetUserByEmail(string email); 
    }
}
