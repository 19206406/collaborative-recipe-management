using FastEndpoints;
using MediatR;

namespace User.API.Features.User.GetUserById
{
    public record GetUserByIdRequest(int Id); 
    public class GetUserByIdEndpoint : Endpoint<GetUserByIdRequest, GetUserByIdResponse>
    {
        private readonly IMediator _mediator;

        public GetUserByIdEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("api/users/{id}/basic");
            Summary(x =>
            {
                x.Summary = "Get basic user information";
                x.Description = "Returns basic user information intended for internal consumption by other services within the application.";
            });
            Description(x => x.WithTags("Users"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(GetUserByIdRequest req, CancellationToken ct)
        {
            var query = new GetUserByIdQuery(req.Id);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
