using Recipe.API.Entities;

namespace Recipe.API.Repositories.TagRepository
{
    public interface ITagRepository
    {
        Task<bool> UpdateTags(List<RecipeTag> items); 
    }
}
