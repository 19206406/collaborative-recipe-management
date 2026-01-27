using BuildingBlocks.CQRS;
using User.API.PasswordHash;
using User.API.repositories.UserRespository;

namespace User.API.Features.User.LoginUser
{
    public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHashService;

        public LoginUserCommandHandler(IUserRepository userRepository, IPasswordHashService passwordHashService)
        {
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
        }

        public async Task<LoginUserResponse> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(command.Email);

            if (user is null)
                throw new Exception();

            var verifyPassword = _passwordHashService.VerifyPassword(user.PasswordHash, command.Password);

            if (!verifyPassword)
                throw new Exception();

            var result = new LoginUserResponse(user.Id, user.Name, user.Email, user.CreatedAt, user.IsActive);

            return result; 
        }
    }
}
