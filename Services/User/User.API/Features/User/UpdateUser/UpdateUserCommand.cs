using BuildingBlocks.CQRS;

namespace User.API.Features.User.UpdateUser
{
    public record UpdateUserCommand(int Id, string Name, string Email, byte IsActive) : ICommand<UpdateUserResponse>; 
}
