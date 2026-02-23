using BuildingBlocks.CQRS;
using Recommendation.API.Common.Cache;
using Recommendation.API.Common.Dtos;
using Recommendation.API.Features.Clients.RecipeClient;

namespace Recommendation.API.Features.Recommendation.GetSimilarRecipes
{
    public class GetSimilarRecipesQueryHandler : IQueryHandler<GetSimilarRecipesQuery, List<RecipeDto>>
    {
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cache;
        private readonly IRecipeServiceClient _recipeService;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public GetSimilarRecipesQueryHandler(HttpClient httpClient, ICacheService cache, IRecipeServiceClient recipeService)
        {
            _httpClient = httpClient;
            _cache = cache;
            _recipeService = recipeService;
        }

        public Task<List<RecipeDto>> Handle(GetSimilarRecipesQuery query, CancellationToken cancellationToken)
        {
        }
    }
}
