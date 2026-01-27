using BuildingBlocks.CQRS;
using User.API.repositories.UserRespository;
using User.API.Services.PasswordHash;

namespace User.API.Features.User.RegisterUser
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHash;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHashService passwordHash)
        {
            _userRepository = userRepository;
            _passwordHash = passwordHash;
        }


        public async Task<RegisterUserResponse> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {

            var passwordHashed = _passwordHash.HashPassword(command.Password); 

            var newUser = new Entities.User
            {
                Name = command.Name,
                Email = command.Email,
                PasswordHash = passwordHashed,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = 1,
            };

            var result = await _userRepository.AddUser(newUser);

            return new RegisterUserResponse(result); 
        }
    }
}
