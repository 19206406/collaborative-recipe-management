using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Microsoft.Extensions.Options;
using User.API.Models;
using User.API.repositories.UserRespository;
using User.API.Services.Jwt;
using User.API.Services.PasswordHash;

namespace User.API.Features.User.LoginUser
{
    public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;

        public LoginUserCommandHandler(
            IUserRepository userRepository, 
            IPasswordHashService passwordHashService, 
            IJwtService jwtService, 
            IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
            _jwtService = jwtService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<LoginUserResponse> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(command.Email);

            if (user is null)
                throw new UnauthorizedException("Usuario no autorizado"); 

            var verifyPassword = _passwordHashService.VerifyPassword(user.PasswordHash, command.Password);

            if (!verifyPassword)
                throw new UnauthorizedException("Usuario no autorizado");

            // generar token 
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var userLogin = new UserLogin(user.Id, user.Name, user.Email, user.CreatedAt, user.IsActive); 

            var result = new LoginUserResponse(accessToken, refreshToken, userLogin);

            return result; 
        }
    }
}
