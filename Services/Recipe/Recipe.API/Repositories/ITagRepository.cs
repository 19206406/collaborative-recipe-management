using Recipe.API.Entities;

namespace Recipe.API.Repositories
{
    public interface ITagRepository
    {
        Task<bool> UpdateTags(List<RecipeTag> items); 
    }
}
