using BuildingBlocks.CQRS;
using Notification.API.Repositories.NotificationRepository;
using System.Runtime.ConstrainedExecution;

namespace Notification.API.Features.Notification.AddReceivedRatingNotification
{
    public record AddReceivedRatingNotificationCommandHandler
        : ICommandHandler<AddReceivedRatingNotificationCommand, AddReceivedRatingNotificationResponse>
    {
        private readonly INotificationRepository _notificationRepository;

        public AddReceivedRatingNotificationCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        //1. **Rating Recibido** (llamado por Rating Service):
        //    - Recibir: { recipeId, ratingValue, userId(quien calificó) }
        //    - ** LLAMAR a Recipe Service** → obtener recipe.user_id y recipe.title
        //    - **LLAMAR a User Service** → obtener nombre de quien calificó
        //    - Crear notificación para el dueño de la receta:

        //    ```
        //     "{userName} calificó tu receta '{recipeTitle}' con {ratingValue} estrellas"
        //    ```

        //    - Si usuario tiene emailNotifications = true → enviar email (simulado con log)

        public Task<AddReceivedRatingNotificationResponse> Handle(AddReceivedRatingNotificationCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
