using BuildingBlocks.Jwt.Claims;
using FastEndpoints;
using MediatR;

namespace User.API.Features.User.GetUser
{
    public record GetUserRequest(int Id); 

    public class GetUserEndpoint : EndpointWithoutRequest<GetUserResponse>
    {
        private readonly IMediator _mediator;

        public GetUserEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public const string Route = "api/users/profile";

        public override void Configure()
        {
            Get("api/users/profile");
            Summary(s =>
            {
                s.Summary = "Obtener usuario autenticado";
                s.Description = "Obtener usuario autenticado";
            });
            Description(x => x.WithTags("Users"));
            //AllowAnonymous(); 
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userId = HttpContext.User.GetUserId();

            var query = new GetUserQuery(userId);
            var result = await _mediator.Send(query); 
            await Send.OkAsync(result); 
        }
    }
}
