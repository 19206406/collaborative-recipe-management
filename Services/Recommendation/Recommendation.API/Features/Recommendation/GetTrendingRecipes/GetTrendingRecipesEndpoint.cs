using FastEndpoints;
using MediatR;
using Recommendation.API.Common.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Recommendation.API.Features.Recommendation.GetTrendingRecipes
{
    public class GetTrendingRecipesEndpoint : EndpointWithoutRequest<List<RecipeDto>>
    {
        private readonly IMediator _mediator;

        public GetTrendingRecipesEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        } 

        public override void Configure()
        {
            Get("/api/recommendations/trending");
            Summary(x =>
            {
                x.Summary = "obtener top recetas";
                x.Description = "Obtener las 20 recetas con mayor trending";
            });
            Description(x => x.WithTags("Recommendations"));
            AllowAnonymous(); 
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var query = new GetTrendingRecipesQuery();
            var recipes = await _mediator.Send(query);

            await Send.OkAsync(recipes); 
        }
    }
}
