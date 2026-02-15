using BuildingBlocks.Exceptions;
using BuildingBlocks.Jwt.Claims;
using FastEndpoints;
using MediatR;

namespace Recipe.API.Features.Recipe.UpdateRecipe
{
    public record UpdateRecipe(string Title, string Description, int PrepTimeMinutes, int CookTimeMinutes, int Difficulty, int Servings, string ImageUrl);
    public record UpdateIngredient(int Id, string Name, decimal Quantity, string Unit, int DisplayOrder);
    public record UpdateStep(int Id, int RecipeId, int StepNumber, string Instruction);
    public record UpdateRecipeRequest(int Id, UpdateRecipe Recipe, List<UpdateIngredient> Ingredients, List<UpdateStep> Steps); 

    public class UpdateRecipeEndpoint : Endpoint<UpdateRecipeRequest, UpdateRecipeResponse>
    {
        private readonly IMediator _mediator;

        public UpdateRecipeEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("api/recipes/{id}");
            Summary(x =>
            {
                x.Summary = "Actualizar una receta";
                x.Description = "Actualizar una receta con todas sus relaciones";
            });
            Description(x => x.WithTags("Recipes")); 
        }

        public override async Task HandleAsync(UpdateRecipeRequest req, CancellationToken ct)
        {
            var userId = HttpContext.User.GetUserId();

            var command = new UpdateRecipeCommand(req.Id, userId, req.Recipe, req.Ingredients, req.Steps);
            var result = await _mediator.Send(command);

            await Send.OkAsync(); 
        }
    }
}
