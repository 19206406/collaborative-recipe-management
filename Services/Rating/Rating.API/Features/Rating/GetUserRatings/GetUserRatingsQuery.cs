using BuildingBlocks.CQRS;

namespace Rating.API.Features.Rating.GetUserRatings
{
    public record GetUserRatingsQuery(int UserId) : IQuery<GetUserRatingsResponse>; 
}
