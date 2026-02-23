using FastEndpoints;
using MediatR;
using Recommendation.API.Common.Dtos;

namespace Recommendation.API.Features.Recommendation.GetPersonalizedRecipes
{
    // TODO: Necesito investigar como paso el jwt a los endpoints que requieren de autenticación y del userId 

    public record GetPersonalizedRecipesRequest(int userId); 
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
        }

        public override async Task HandleAsync(GetPersonalizedRecipesRequest req, CancellationToken ct)
        {
            var query = new GetPersonalizedRecipesQuery(req.userId);
            var result = await _mediator.Send(query);

            await Send.OkAsync(result); 
        }
    }
}
