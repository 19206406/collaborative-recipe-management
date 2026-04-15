using FastEndpoints;
using MediatR;
using Recommendation.API.Common.Dtos;

namespace Recommendation.API.Features.Recommendation.GetPersonalizedRecipes
{
    public record GetPersonalizedRecipesRequest(int UserId); 
    public class GetPersonalizedRecipesEndpoint : Endpoint<GetPersonalizedRecipesRequest, List<RecipeDto>>
    {
        private readonly IMediator _mediator;

        public GetPersonalizedRecipesEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("api/recommendations/user/{userId}");
            Summary(x =>
            {
                x.Summary = "Recetas personalizadas";
                x.Description = "Obtiene recetas personalizadas para cada uno de los usuarios";
            });
            Description(x => x.WithTags("Recommendations"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(GetPersonalizedRecipesRequest req, CancellationToken ct)
        {
            var query = new GetPersonalizedRecipesQuery(req.UserId);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
