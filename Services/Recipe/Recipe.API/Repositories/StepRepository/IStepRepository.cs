using Recipe.API.Entities;

namespace Recipe.API.Repositories.StepRepository
{
    public interface IStepRepository
    {
        Task<List<Step>> GetStepsByIdRecipeAsync(int recipeId);
        Task AddStepAsync(Step step);
        Task UpdateStepAsync(Step step);
        Task DeleteStepAsync(int id); 
        Task<bool> RemoveSteps(List<Entities.Step> items);
        Task<bool> UpdateSteps(List<Entities.Step> items);
        Task<bool> AddSteps(List<Entities.Step> items, int lenSteps); 
    }
}
