using BuildingBlocks.CQRS;

namespace User.API.Features.User.LoginUser
{
    public record LoginUserCommand(string Email, string Password) : ICommand<LoginUserResponse>; 
}
