using BuildingBlocks.Jwt.Claims;
using FastEndpoints;
using MediatR;
using InvalidOperationException = BuildingBlocks.Exceptions.InvalidOperationException;

namespace User.API.Features.User.DeleteUser
{
    public record DeleteUserRequest(int Id); 

    public class DeleteUserEndpoint : Endpoint<DeleteUserRequest>
    {
        private readonly IMediator _mediator;

        public DeleteUserEndpoint(IMediator mediator)
        {
           _mediator = mediator;
        }

        public override void Configure()
        {
            Delete("/api/users/{id}");
            Summary(s =>
            {
                s.Summary = "Eliminar usuario del sistema";
                s.Description = "Eliminar usuario del sistema";
            });
            Description(x => x.WithTags("Users"));
        }

        public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
        {
            var userId = HttpContext.User.GetUserId();

            if (userId != req.Id)
                throw new InvalidOperationException("No tienes permiso de ejectar esta acción verifica la acción"); 
            var command = new DeleteUserCommand(req.Id);
            var result = await _mediator.Send(command);

            await Send.NoContentAsync(); 
        }
    }
}
