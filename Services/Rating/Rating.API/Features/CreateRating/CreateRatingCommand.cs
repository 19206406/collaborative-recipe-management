using BuildingBlocks.CQRS;

namespace Rating.API.Features.CreateRating
{
    public record CreateRatingCommand(int UserId, int RecipeId, int Rating, string? Comment) : ICommand<CreateRatingResponse>; 
}
