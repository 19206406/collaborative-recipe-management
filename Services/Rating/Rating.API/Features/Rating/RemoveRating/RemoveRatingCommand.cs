using BuildingBlocks.CQRS;

namespace Rating.API.Features.Rating.RemoveRating
{
    public record RemoveRatingCommand(int Id) : ICommand;
}
