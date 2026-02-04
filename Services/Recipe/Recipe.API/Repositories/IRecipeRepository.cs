namespace Recipe.API.Repositories
{
    public interface IRecipeRepository
    {
        Task<Entities.Recipe> AddRecipe(Entities.Recipe recipe);
        Task<Entities.Recipe> GetRecipe(int id);
        Task<List<Entities.Recipe>> GetRecipesByUser(int userId); 
        Task RemoveRecipe(int id);
        Task<long> NumberOfItems();
        Task<List<Entities.Recipe>> GetRecipePagination(int pageNumber, int pageSize);

        Task<bool> UpdateRecipeOnly(Entities.Recipe recipe);
    }
}
