using BuildingBlocks.CQRS;

namespace User.API.Features.User.GetUser
{
    public record GetUserQuery(int Id) : IQuery<GetUserResponse>; 
}
