using User.API.Entities;

namespace User.API.repositories.UserPreferenceRepository
{
    public interface IUserPreferenceRespository
    {
        Task<List<UserPreference>> GetPreferences(); 

        Task<Entities.User> GetUserPreferences(int id);
        Task<bool> AddUserPreferences(int userId, List<string> items);
        Task<bool> RemoveReferences(List<UserPreference> items);
        Task<bool> UpdateReferences(List<UserPreference> items);

        //Task<ICollection<Entities.User>> GetPreferences(); 
    }
}
