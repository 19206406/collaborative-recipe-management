using FastEndpoints;
using MediatR;

namespace Recipe.API.Features.Recipe.UpdateRecipeRating
{
    public record UpdateRecipeRatingRequest(int Id, decimal NewAverage, int NewRatingCount); 
    public class UpdateRecipeRatingEndpoint : Endpoint<UpdateRecipeRatingRequest, UpdateRecipeRatingResponse>
    {
        private readonly IMediator _mediator;

        public UpdateRecipeRatingEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("/api/recipes/{id}/rating");
            Summary(x =>
            {
                x.Summary = "Update recipe internal rating";
                x.Description = "Updates the internal rating score of a recipe. This endpoint is used to sync the recipe's cached rating value.";
            });
            Description(x => x.WithTags("Recipes"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(UpdateRecipeRatingRequest req, CancellationToken ct)
        {
            var command = new UpdateRecipeRatingCommand(req.Id, req.NewAverage, req.NewRatingCount);
            var result = await _mediator.Send(command);

            await Send.OkAsync(result); 
        }
    }
}
