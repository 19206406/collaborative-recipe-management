using FastEndpoints;
using MediatR;

namespace Recipe.API.Features.Recipe.UpdateRecipeRating
{
    public record UpdateRecipeRatingRequest(int Id, decimal Rating); 
    public class UpdateRecipeRatingEndpoint : Endpoint<UpdateRecipeRatingRequest, UpdateRecipeRatingResponse>
    {
        private readonly IMediator _mediator;

        public UpdateRecipeRatingEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("api/recipes/{id}/rating");
            Summary(x =>
            {
                x.Summary = "Actualizar el rating de una receta";
                x.Description = "Permite actualizar el rating de una receta de una persona sin importar si es nuestra";
            }); 
            Description(x => x.WithTags("Recipes")); 
        }

        public override async Task HandleAsync(UpdateRecipeRatingRequest req, CancellationToken ct)
        {
            var command = new UpdateRecipeRatingCommand(req.Id, req.Rating);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result); 
        }
    }
}
