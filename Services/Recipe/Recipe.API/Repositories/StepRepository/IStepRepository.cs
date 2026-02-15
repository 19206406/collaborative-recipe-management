namespace Recipe.API.Repositories.StepRepository
{
    public interface IStepRepository
    {
        Task<bool> RemoveSteps(List<Entities.Step> items);
        Task<bool> UpdateSteps(List<Entities.Step> items);
        Task<bool> AddSteps(List<Entities.Step> items); 
    }
}
