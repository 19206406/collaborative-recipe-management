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

            //var userByEmail = await _userRepository.GetUserByEmail(command.Email);

            //if (userByEmail is not null)
            //    throw new ConflictException("No se puede actualizar por favor intenta con otros valores"); 


            user.Name = command.Name;
            user.Email = command.Email;
            user.IsActive = command.IsActive;
            user.UpdatedAt = DateTime.UtcNow; 

            var updatedUser = await _userRepository.UpdateUser(user);

            return new UpdateUserResponse(updatedUser.Id, updatedUser.Name, updatedUser.Email, 
                updatedUser.CreatedAt, updatedUser.UpdatedAt, updatedUser.IsActive); 
        }
    }
}
