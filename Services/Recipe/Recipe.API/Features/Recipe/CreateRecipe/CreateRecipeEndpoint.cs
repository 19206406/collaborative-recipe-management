using BuildingBlocks.Jwt.Claims;
using FastEndpoints;
using MediatR;
using Recipe.API.Features.Recipe.GetRecipe;

namespace Recipe.API.Features.Recipe.CreateRecipe
{
    public record CreateRecipe(string Title, string Description, int PrepTimeMinutes, int CookTimeMinutes, string Difficulty, int Servings, string? ImageUrl);
    public record CreateIngredient(string Name, decimal Quantity, string Unit, int DisplayOrder); 
    public record CreateStep(string Instruction);
    public record CreateRecipeRequest(CreateRecipe Recipe, List<CreateIngredient> Ingredients, List<CreateStep> Steps); 

    public class CreateRecipeEndpoint : Endpoint<CreateRecipeRequest, CreateRecipeResponse>
    {
        private readonly IMediator _mediator;

        public CreateRecipeEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/api/recipes");
            Summary(x =>
            {
                x.Summary = "Crear receta";
                x.Description = "Crear receta con todas sus relaciones ('pasos', ingredientes, tags')";
            });
            Description(x => x.WithTags("Recipes")); 
        }

        public override async Task HandleAsync(CreateRecipeRequest req, CancellationToken ct)
        {
            var userId = HttpContext.User.GetUserId();

            var command = new CreateRecipeCommand(userId, req.Recipe, req.Ingredients, req.Steps);
            var result = await _mediator.Send(command);

            await Send.CreatedAtAsync(GetRecipeEndpoint.Route, new { id = result.Recipe.Id }, result); 
        }
    }
}
