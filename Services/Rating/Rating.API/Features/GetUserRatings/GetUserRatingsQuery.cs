using BuildingBlocks.CQRS;

namespace Rating.API.Features.GetUserRatings
{
    public record GetUserRatingsQuery(int UserId) : IQuery<GetUserRatingsResponse>; 
}
