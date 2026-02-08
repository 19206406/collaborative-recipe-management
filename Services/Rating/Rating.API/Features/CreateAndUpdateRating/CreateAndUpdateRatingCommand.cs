using BuildingBlocks.CQRS;

namespace Rating.API.Features.CreateAndUpdateRating
{
    public record CreateAndUpdateRatingCommand(int Id, int UserId, int RecipeId, int Rating, string? Comment, bool IsToUpdate) 
        : ICommand<CreateAndUpdateRatingResponse>; 
}
