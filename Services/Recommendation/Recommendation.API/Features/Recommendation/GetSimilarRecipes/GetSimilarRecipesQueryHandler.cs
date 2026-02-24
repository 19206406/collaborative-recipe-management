using BuildingBlocks.CQRS;
using Mapster;
using Recommendation.API.Common.Cache;
using Recommendation.API.Common.Dtos;
using Recommendation.API.Features.Clients.RecipeClient;

namespace Recommendation.API.Features.Recommendation.GetSimilarRecipes
{
    public class GetSimilarRecipesQueryHandler : IQueryHandler<GetSimilarRecipesQuery, List<RecipeDto>>
    {
        private readonly ICacheService _cache;
        private readonly IRecipeServiceClient _recipeService;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public GetSimilarRecipesQueryHandler(ICacheService cache, IRecipeServiceClient recipeService)
        {
            _cache = cache;
            _recipeService = recipeService;
        }

        public async Task<List<RecipeDto>> Handle(GetSimilarRecipesQuery query, CancellationToken cancellationToken)
        {
            var cacheKey = $"similar:{query.recipeId}";

            var cached = await _cache.GetAsync<List<RecipeDto>>(cacheKey);
            if (cached is not null)
                return cached;

            var recipe = await _recipeService.GetRecipeAsync(query.recipeId, cancellationToken);
            if (recipe is null)
                return [];

            var tags = recipe.Tags.Select(r => r.Tag).ToList();
            var similar = await _recipeService.GetPersonalizedRecipesAsync(tags);

            await _cache.SetAsync(cacheKey, similar, CacheDuration);

            return similar.Adapt<List<RecipeDto>>(); 
        }
    }
}
