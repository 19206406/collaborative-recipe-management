using BuildingBlocks.Jwt.Claims;
using FastEndpoints;
using MediatR;

namespace Recipe.API.Features.Recipe.RemoveRecipe
{
    public record RemoveRecipeRequest(int Id); 
    public class RemoveRecipeEndpoint : Endpoint<RemoveRecipeRequest>
    {
        private readonly IMediator _mediator;

        public RemoveRecipeEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Delete("/api/recipes/{id}");
            Summary(x =>
            {
                x.Summary = "Delete recipe";
                x.Description = "Permanently deletes a recipe. Only the user who created the recipe is authorized to perform this action.";
            });
            Description(x => x.WithTags("Recipes")); 
        }

        public override async Task HandleAsync(RemoveRecipeRequest req, CancellationToken ct)
        {

            var userId = HttpContext.User.GetUserId(); 

            var command = new RemoveRecipeCommand(req.Id, userId);
            var result = await _mediator.Send(command);

            await Send.NoContentAsync(); 
        }
    }
}
