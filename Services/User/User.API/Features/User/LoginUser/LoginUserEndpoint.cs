using FastEndpoints;
using MediatR;

namespace User.API.Features.User.LoginUser
{
    public record LoginUserRequest(string Email, string Password); 

    public class LoginUserEndpoint : Endpoint<LoginUserRequest, LoginUserResponse>
    {
        private readonly IMediator _mediator;

        public LoginUserEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("api/users/login");
            Summary(s =>
            {
                s.Summary = "Login del usuario";
                s.Description = "Login del usuario para el ingreso al sistema";
            });
            Description(x => x.WithTags("Users"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(LoginUserRequest req, CancellationToken ct)
        {
            var command = new LoginUserCommand(req.Email, req.Password);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result); 
        }
    }
}
