using BuildingBlocks.CQRS;
using Recommendation.API.Common.Cache;
using Recommendation.API.Common.Dtos;
using Recommendation.API.Features.Clients.RecipeClient;
using System.Net.Mail;

namespace Recommendation.API.Features.Recommendation.GetTrendingRecipes
{
    public class GetTrendingRecipesQueryHandler : IQueryHandler<GetTrendingRecipesQuery, List<RecipeDto>>
    {
        private readonly ICacheService _cache;
        private readonly IRecipeServiceClient _recipeService;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);
        private const string CacheKey = "trending:top20";

        public GetTrendingRecipesQueryHandler(ICacheService cache, IRecipeServiceClient recipeService)
        {
            _cache = cache;
            _recipeService = recipeService;
        }

        public async Task<List<RecipeDto>> Handle(GetTrendingRecipesQuery request, CancellationToken cancellationToken)
        {
            var cached = await _cache.GetAsync<List<RecipeDto>>(CacheKey);
            if (cached is not null) 
                return cached;

            var recipes = await _recipeService.GetTopRecipes();

            // la formula para calcular el score o el trending se aplica directamente en el servicio de recipe 
            // para que de esta forma sea mucho más eficiente trayendo los elementos y calcular esto. 

            await _cache.SetAsync(CacheKey, recipes, CacheDuration); 

            return recipes; 

        }
    }
}
