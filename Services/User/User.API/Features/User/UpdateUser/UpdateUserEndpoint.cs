using BuildingBlocks.Jwt.Claims;
using FastEndpoints;
using MediatR;

namespace User.API.Features.User.UpdateUser
{
    public record UpdateUserRequest(string Name, string Email, byte IsActive); 

    public class UpdateUserEndpoint : Endpoint<UpdateUserRequest, UpdateUserResponse>
    {
        private readonly IMediator _mediator;

        public UpdateUserEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("/api/users/profile");
            Summary(s =>
            {
                s.Summary = "Actualizar un usuario";
                s.Description = "Actualizar la información basica de un usuario";
            }); 
            Description(x => x.WithTags("Users")); 
        }

        public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
        {
            var userId = HttpContext.User.GetUserId(); 
            var command = new UpdateUserCommand(userId, req.Name, req.Email, req.IsActive);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result); 
        }
    }
}
