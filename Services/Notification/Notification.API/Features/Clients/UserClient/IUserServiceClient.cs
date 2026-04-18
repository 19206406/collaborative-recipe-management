using Notification.API.Common.Dtos;

namespace Notification.API.Features.Clients.UserClient
{
    public interface IUserServiceClient
    {
        Task<UserDto> UserRecipeRatingAsync(int userId); 
    }
}
