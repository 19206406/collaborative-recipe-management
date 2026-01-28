using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using MediatR;
using User.API.repositories.UserRespository;

namespace User.API.Features.User.DeleteUser
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUser(command.Id);

            if (user is null)
                throw new NotFoundException("usuario", command.Id); 

            await _userRepository.DeleteUser(user);

            return Unit.Value; 
        }
    }
}
