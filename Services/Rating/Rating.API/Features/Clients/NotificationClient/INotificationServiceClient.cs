using Rating.API.Common.Dtos;

namespace Rating.API.Features.Clients.NotificationClient
{
    public interface INotificationServiceClient
    {
        Task<bool> CreateNewNotificationAsync(CreateNotificationRequest request); 
    }
}
