namespace User.API.repositories.UserPreferenceRepository
{
    public interface IUserPreferenceRespository<T> where T : class 
    {
        Task AddPreference(T entity);
        Task<ICollection<Entities.User>> GetPreferences(); 
        Task<Entities.User> GetPreference(int id);
        Task DeletePreference(int id);
        Task UpdatePreference(T entity);
    }
}
