using BuildingBlocks.CQRS;
using Mapster;
using Notification.API.Features.Clients.RecipeClient;
using Notification.API.Features.Clients.UserClient;
using Notification.API.Repositories.NotificationRepository;
using System.Runtime.ConstrainedExecution;

namespace Notification.API.Features.Notification.AddReceivedRatingNotification
{
    public record AddReceivedRatingNotificationCommandHandler
        : ICommandHandler<AddReceivedRatingNotificationCommand, AddReceivedRatingNotificationResponse>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IRecipeServiceClient _recipeServiceClient;
        private readonly IUserServiceClient _userServiceClient;
        private readonly ILogger<AddReceivedRatingNotificationCommandHandler> _logger;

        public AddReceivedRatingNotificationCommandHandler(
            INotificationRepository notificationRepository, IRecipeServiceClient recipeServiceClient, 
            IUserServiceClient userServiceClient, ILogger<AddReceivedRatingNotificationCommandHandler> logger)
        {
            _notificationRepository = notificationRepository;
            _recipeServiceClient = recipeServiceClient;
            _userServiceClient = userServiceClient;
            _logger = logger;
        }

        //1. **Rating Recibido** (llamado por Rating Service):
        //    - Recibir: { recipeId, ratingValue, userId(quien calificó) } -- listo 
        //    - ** LLAMAR a Recipe Service** → obtener recipe.user_id y recipe.title
        //    - **LLAMAR a User Service** → obtener nombre de quien calificó -- listo 
        //    - Crear notificación para el dueño de la receta: 

        //    ```
        //     "{userName} calificó tu receta '{recipeTitle}' con {ratingValue} estrellas" -- listo 
        //    ```

        //    - Si usuario tiene emailNotifications = true → enviar email (simulado con log)

        public async Task<AddReceivedRatingNotificationResponse> Handle(AddReceivedRatingNotificationCommand command, CancellationToken cancellationToken)
        {

            var recipe = await _recipeServiceClient.RecipeByIdAsync(command.RecipeId);
            var user = await _userServiceClient.UserRecipeRatingAsync(recipe.UserId);

            var newNotification = new Entities.Notification
            {
                UserId = user.Id, 
                Type = "rating_received", 
                Title = "Nueva calificación", 
                Message = $"{user.Name} calificó tu receta '{recipe.Title}' con {recipe.AverageRating} estrellas", 
                IsRead = 0, 
                CreatedAt = DateTime.UtcNow
            };

            // TODO: Si usuario tiene emailNotifications = true → enviar email (simulado con log) 
            // se podria implementar el cervicio de mensajeria por email

            await _notificationRepository.AddNotificationAsync(newNotification);

            return newNotification.Adapt<AddReceivedRatingNotificationResponse>(); 
        }
    }
}
