using Recipe.API.Entities;

namespace Recipe.API.Repositories.TagRepository
{
    public interface ITagRepository
    {
        Task<List<RecipeTag>> GetTagsByIdRecipeAsync(int recipeId); 
        Task AddTagAsync(RecipeTag tag);
        Task UpdateTagAsync(RecipeTag tag);
        Task DeleteTagAsync(int id); 
    }
}
