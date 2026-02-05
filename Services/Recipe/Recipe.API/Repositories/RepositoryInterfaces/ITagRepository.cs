using Recipe.API.Entities;

namespace Recipe.API.Repositories.RepositoryInterfaces
{
    public interface ITagRepository
    {
        Task<bool> UpdateTags(List<RecipeTag> items); 
    }
}
