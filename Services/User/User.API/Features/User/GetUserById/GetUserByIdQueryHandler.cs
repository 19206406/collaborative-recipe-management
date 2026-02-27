using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using User.API.repositories.UserRespository;

namespace User.API.Features.User.GetUserById
{
    public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, GetUserByIdResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUser(query.Id);

            if (user is null)
                throw new NotFoundException("usuario", query.Id);

            return new GetUserByIdResponse(user.Id, user.Name, user.Email); 
        }
    }
}
