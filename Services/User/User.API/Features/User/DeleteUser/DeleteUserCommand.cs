using BuildingBlocks.CQRS;

namespace User.API.Features.User.DeleteUser
{
    public record DeleteUserCommand(int Id) : ICommand; 
}
