using BuildingBlocks.CQRS;

namespace Recipe.API.Features.Recipe.RemoveRecipe
{
    public record RemoveRecipeCommand(int Id, int UserId) : ICommand; 
}
