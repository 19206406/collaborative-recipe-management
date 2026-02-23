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

            // exluir recetas ya calificadas por el usuario 
            var alreadyRatedIds = userRatings.Select(r => r.RecipeId).ToHashSet();

            // obtener recetas que coincidan con las preferencias del usuario 
            // debo de hacer que el recipeService - search acepte preferencias o algo así 

            // valido si no viene ninguna preferencia en el client aunque tambien deberia de haber un validator 
            var personalized = await _recipeService.GetPersonalizedRecipesAsync(preferencesMap);

            await _cache.SetAsync(cacheKey, personalized, CacheDuration);

            return personalized.Adapt<List<RecipeDto>>(); 
        }
    }
}
