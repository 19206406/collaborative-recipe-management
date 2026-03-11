using FastEndpoints;
using MediatR;

namespace Recipe.API.Features.Recipe.GetRecipesByUser
{
    public record GetRecipeByUserRequest(int UserId); 
    public class GetRecipesByUserEndpoint : Endpoint<GetRecipeByUserRequest, GetRecipesByUserResponse>
    {
        private readonly IMediator _mediator;

        public GetRecipesByUserEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("/api/recipes/user/{userId}");
            Summary(x =>
            {
                x.Summary = "obtener todas las recetas de un usuario";
                x.Description = "obtener todas las recetas de un usuario";
            });
            Description(x => x.WithTags("Recipes"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(GetRecipeByUserRequest req, CancellationToken ct)
        {
            // TODO: verificar que el usuario si exista esto se hace por medio del otro servico 
            var query = new GetRecipesByUserQuery(req.UserId);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
