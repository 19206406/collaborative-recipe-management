using BuildingBlocks.CQRS;

namespace Rating.API.Features.Rating.CreateRating
{
    public record CreateRatingCommand(int UserId, int RecipeId, int Rating, string? Comment) : ICommand<CreateRatingResponse>; 
}
