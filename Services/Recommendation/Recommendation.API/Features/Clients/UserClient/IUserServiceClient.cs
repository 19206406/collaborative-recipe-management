using Recommendation.API.Common.Dtos;

namespace Recommendation.API.Features.Clients.UserClient
{
    public interface IUserServiceClient
    {
        Task<bool> UserExistByIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<UserPreferencesDto> GetUserPreferencesAsync(int userId, CancellationToken cancellationToken = default); 

    }
}
