using BuildingBlocks.CQRS;
using Mapster;
using Notification.API.Features.Clients.RecipeClient;
using Notification.API.Features.Clients.UserClient;
using Notification.API.Repositories.NotificationPreferenceRepository;
using Notification.API.Repositories.NotificationRepository;
using System.Runtime.ConstrainedExecution;

namespace Notification.API.Features.Notification.AddReceivedRatingNotification
{
    public record AddReceivedRatingNotificationCommandHandler
        : ICommandHandler<AddReceivedRatingNotificationCommand, AddReceivedRatingNotificationResponse>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationPreferenceRepository _notificationPreferenceRepository;
        private readonly IRecipeServiceClient _recipeServiceClient;
        private readonly IUserServiceClient _userServiceClient;
        private readonly ILogger<AddReceivedRatingNotificationCommandHandler> _logger;

        public AddReceivedRatingNotificationCommandHandler(
            INotificationRepository notificationRepository, INotificationPreferenceRepository notificationPreferenceRepository, 
            IRecipeServiceClient recipeServiceClient, 
            IUserServiceClient userServiceClient, ILogger<AddReceivedRatingNotificationCommandHandler> logger)
        {
            _notificationRepository = notificationRepository;
            _notificationPreferenceRepository = notificationPreferenceRepository;
            _recipeServiceClient = recipeServiceClient;
            _userServiceClient = userServiceClient;
            _logger = logger;
        }

        public async Task<AddReceivedRatingNotificationResponse> Handle(AddReceivedRatingNotificationCommand command, CancellationToken cancellationToken)
        {

            var recipe = await _recipeServiceClient.RecipeByIdAsync(command.RecipeId);

            var user = await _userServiceClient.UserRecipeRatingAsync(recipe.Recipe.UserId);

            var newNotification = new Entities.Notification
            {
                UserId = user.Id, 
                Type = "rating_received", 
                Title = "Nueva calificación", 
                Message = $"{user.Name} calificó tu receta '{recipe.Recipe.Title}' con {command.RatingValue} estrellas", 
                IsRead = 0, 
                CreatedAt = DateTime.UtcNow
            };

            var notificationPreferences = await _notificationPreferenceRepository.GetPreferencesByUserIdAsync(command.UserId); 

            if (notificationPreferences is not null && notificationPreferences.EmailNotifications == 1)
            {
                _logger.LogInformation($"Email de calificacion de receta: " +
                    $"{user.Name} calificó tu receta '{recipe.Recipe.Title}' con {command.RatingValue} estrellas"); 
            } 

            await _notificationRepository.AddNotificationAsync(newNotification);

            return newNotification.Adapt<AddReceivedRatingNotificationResponse>(); 
        }
    }
}
