using BuildingBlocks.CQRS;

namespace User.API.Features.User.GetUserById
{
    public record GetUserByIdQuery(int Id) : IQuery<GetUserByIdResponse>; 
}
