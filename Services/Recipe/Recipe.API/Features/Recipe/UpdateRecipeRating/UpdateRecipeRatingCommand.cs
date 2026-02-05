using BuildingBlocks.CQRS;

namespace Recipe.API.Features.Recipe.UpdateRecipeRating
{
    public record UpdateRecipeRatingCommand(int Id, decimal Rating) : ICommand<UpdateRecipeRatingResponse>; 
}
