using FastEndpoints;
using MediatR;

namespace User.API.Features.User.GetUser
{
    public record GetUserRequest(int Id); 

    public class GetUserEndpoint : Endpoint<GetUserRequest, GetUserResponse>
    {
        private readonly IMediator _mediator;

        public GetUserEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/api/users/{id}");
            Summary(s =>
            {
                s.Summary = "Obtener usuario autenticado";
                s.Description = "Obtener usuario autenticado";
            });
            Description(x => x.WithTags("Users"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(GetUserRequest req, CancellationToken ct)
        {
            //var userId = int.Parse(s: User.FindFirst("userId").Value ?? "0"); 

            var query = new GetUserQuery(req.Id);
            var result = await _mediator.Send(query); 
            await Send.OkAsync(result); 
        }
    }
}
