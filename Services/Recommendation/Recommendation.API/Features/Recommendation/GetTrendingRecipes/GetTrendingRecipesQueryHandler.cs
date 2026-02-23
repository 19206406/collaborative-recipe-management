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
        private const int TopCount = 20; 

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

            var recipes = await _recipeService.GetByIngredientsAsync(new List<string> { });
            
            // formula de trending: score = (average_rating * rating_count) / (days_since_created + 2)

            var trending = recipe

        }

        private static double CalcaulateTrendingScore(RecipeDto recipe)
        {
            var daysSinceCreated = (DateTime.UtcNow - recipe.recipe.CreatedAt).TotalDays;
            return (double)(recipe.recipe.AverageRating * recipe.recipe.RatingCount) / (daysSinceCreated + 2); 
        }
    }
}
