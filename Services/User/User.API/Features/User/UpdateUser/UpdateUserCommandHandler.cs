using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using User.API.repositories.UserRespository;

namespace User.API.Features.User.UpdateUser
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UpdateUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UpdateUserResponse> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUser(command.Id);

            if (user is null)
                throw new NotFoundException("usuario", command.Id); 

            var updateUser = new Entities.User
            {
                Id = user.Id,
                Name = command.Name,
                Email = command.Email,
                PasswordHash = user.PasswordHash,
                IsActive = command.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = DateTime.UtcNow,
            };

            await _userRepository.UpdateUser(updateUser);

            return new UpdateUserResponse(); 
        }
    }
}
