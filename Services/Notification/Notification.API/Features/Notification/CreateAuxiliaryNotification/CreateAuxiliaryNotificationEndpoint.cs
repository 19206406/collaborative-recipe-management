using FastEndpoints;
using MediatR;

namespace Notification.API.Features.Notification.CreateAuxiliaryNotification
{
    public record CreateAuxiliaryNotificationRequest(int UserId, string Type, string Title, string Message); 

    public class CreateAuxiliaryNotificationEndpoint : Endpoint<CreateAuxiliaryNotificationRequest, CreateAuxiliaryNotificationResponse>
    {
        private readonly IMediator _mediator;

        public CreateAuxiliaryNotificationEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/api/notifications");
            Summary(x =>
            {
                x.Summary = "Crear una notificación";
                x.Description = "Crea una notificación de prueba para testear el funcionamiento de los demás endpoints de este servicio";
            });
            Description(x => x.WithTags("Notifications"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(CreateAuxiliaryNotificationRequest req, CancellationToken ct)
        {
            var command = new CreateAuxiliaryNotificationCommand(req.UserId, req.Type, req.Title, req.Message);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result); 
        }
    }
}
