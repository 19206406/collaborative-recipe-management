using FastEndpoints;
using MediatR;

namespace User.API.Features.User.RegisterUser
{
    public record RegisterUserRequest(string Name, string Email, string Password);

    public class RegisterUserEndpoint : Endpoint<RegisterUserRequest, RegisterUserResponse>
    {
        private readonly IMediator _mediator;

        public RegisterUserEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/api/users/register");
            Summary(s =>
            {
                s.Summary = "Registro de un nuevo usuario";
                s.Description = "Registro de un nuevo usuario";
            });
            Description(x => x.WithTags("Users"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(RegisterUserRequest req, CancellationToken ct)
        {
            var command = new RegisterUserCommand(req.Name, req.Email, req.Password);
            var result = await _mediator.Send(command);
            await Send.OkAsync(result); 
        }
    }
}    
