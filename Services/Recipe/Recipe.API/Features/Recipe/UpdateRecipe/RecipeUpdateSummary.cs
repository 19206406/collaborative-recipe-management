namespace Recipe.API.Features.Recipe.UpdateRecipe
{
    public record RecipeUpdateSummary(
        int CreatedIngredientes, int UpdatedIngredients, int RemovedIngredients,
        int CreatedSteps, int UpdatedSteps, int RemovedSteps,
        int CreatedTags, int UpdatedTags, int RemovedTags); 
}
