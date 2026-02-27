using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using User.API.repositories.UserRespository;

namespace User.API.Features.User.GetUser
{
    public class GetUserQueryHandler : IQueryHandler<GetUserQuery, GetUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserResponse> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUser(query.Id);

            if (user is null)
                throw new NotFoundException("usuario", query.Id);

            var userResult = new GetUserResponse(user.Id, user.Name, user.Email, user.CreatedAt, user.UpdatedAt, user.IsActive);

            return userResult; 
        }
    }
}
