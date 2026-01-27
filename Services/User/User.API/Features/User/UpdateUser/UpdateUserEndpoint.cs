using FastEndpoints;
using MediatR;

namespace User.API.Features.User.UpdateUser
{
    public record UpdateUserRequest(int Id, string Name, string Email, byte IsActive); 

    public class UpdateUserEndpoint : Endpoint<UpdateUserRequest, UpdateUserResponse>
    {
        private readonly IMediator _mediator;

        public UpdateUserEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("api/users/{id}");
            Summary(s =>
            {
                s.Summary = "Actualizar un usuario";
                s.Description = "Actualizar la información basica de un usuario";
            }); 
            Description(x => x.WithTags("Users")); 
        }

        public override Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
        {
            var command = new UpdateUserCommand(req.Id, req.Name, req.Email, req.IsActive);
            var result = _mediator.Send(command);

            return Send.OkAsync(); 
        }
    }
}
