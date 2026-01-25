using BuildingBlocks.CQRS;

namespace User.API.Features.User.RegisterUser
{
    public record RegisterUserCommand(string Name, string Email, string Password)
        : ICommand<RegisterUserResult>;
}
