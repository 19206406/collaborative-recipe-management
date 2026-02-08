using BuildingBlocks.CQRS;

namespace Rating.API.Features.RemoveRating
{
    public record RemoveRatingCommand(int Id) : ICommand;
}
