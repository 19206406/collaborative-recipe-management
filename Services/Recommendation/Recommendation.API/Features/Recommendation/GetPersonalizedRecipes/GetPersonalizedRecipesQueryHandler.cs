using BuildingBlocks.CQRS;
using Mapster;
using Recommendation.API.Common.Cache;
using Recommendation.API.Common.Dtos;
using Recommendation.API.Features.Clients.RatingClient;
using Recommendation.API.Features.Clients.RecipeClient;
using Recommendation.API.Features.Clients.UserClient;

namespace Recommendation.API.Features.Recommendation.GetPersonalizedRecipes
{
    public class GetPersonalizedRecipesQueryHandler : IQueryHandler<GetPersonalizedRecipesQuery, List<RecipeDto>>
    {
        private readonly IUserServiceClient _userService;
        private readonly IRecipeServiceClient _recipeService;
        private readonly IRatingServiceClient _ratingService;
        private readonly ICacheService _cache;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public GetPersonalizedRecipesQueryHandler(
            IUserServiceClient userService, 
            IRecipeServiceClient recipeService, 
            IRatingServiceClient ratingService, 
            ICacheService cache)
        {
            _userService = userService;
            _recipeService = recipeService;
            _ratingService = ratingService;
            _cache = cache;
        }

        public async Task<List<RecipeDto>> Handle(GetPersonalizedRecipesQuery query, CancellationToken cancellationToken)
        {
            var cacheKey = $"user:{query.UserId}:recommendations";

            var cached = await _cache.GetAsync<List<RecipeDto>>(cacheKey);
            if (cached is not null)
                return cached;

            // llamar a user service y rating service 
            var preferences = await _userService.GetUserPreferencesAsync(query.UserId);
            var preferencesMap = preferences.Preferences.Select(p => p.PreferenceType).ToList(); 
            var userRatings = await _ratingService.GetRatingsByUserAsync(query.UserId);

            var alreadyRatedIds = userRatings.Select(r => r.RecipeId).ToHashSet();

            var personalized = await _recipeService.GetPersonalizedRecipesAsync(preferencesMap);
            var filteredPersonalizedRecipes = personalized.Select(p => !alreadyRatedIds.Contains(p.Id)); // recetas filtradas

            await _cache.SetAsync(cacheKey, personalized, CacheDuration);

            return filteredPersonalizedRecipes.Adapt<List<RecipeDto>>(); 
        }
    }
}
