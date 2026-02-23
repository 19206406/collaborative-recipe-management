using BuildingBlocks.CQRS;
using Recommendation.API.Common.Cache;
using Recommendation.API.Common.Dtos;
using Recommendation.API.Features.Clients.RecipeClient;
using Recommendation.API.Features.Clients.UserClient;
using System.Security.Cryptography;
using System.Text;

namespace Recommendation.API.Features.Recommendation.RecommendationsByIngredients
{
    public class RecommendationsByIngredientsCommandHandler
        : ICommandHandler<RecommendationsByIngredientsCommand, List<IngredientMatchDto>>
    {
        private readonly IRecipeServiceClient _recipeService;
        private readonly IUserServiceClient _userService;
        private readonly ICacheService _cache;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);
        private const decimal MinMatchPercentage = 60m;

        public RecommendationsByIngredientsCommandHandler(
            IRecipeServiceClient recipeService,
            IUserServiceClient userService, ICacheService cache)
        {
            _recipeService = recipeService;
            _userService = userService;
            _cache = cache;
        }

        public async Task<List<IngredientMatchDto>> Handle(RecommendationsByIngredientsCommand command, CancellationToken cancellationToken)
        {
            // generar cache key única basada en los ingredientes
            var sortedIngredients = command.Ingredients
                .Select(i => i.ToLowerInvariant().Trim())
                .OrderBy(i => i)
                .ToList();

            var hash = ComputeHash(sortedIngredients);
            var cacheKey = $"ingredients:{hash}";

            // intentar obtener el cache primero por si ya existe 
            var cached = await _cache.GetAsync<List<IngredientMatchDto>>(cacheKey);
            if (cached is not null)
                return cached;

            // llamar a recipe service y a user service 
            var recipes = await _recipeService.GetByIngredientsAsync(sortedIngredients);
            var preferences = await _userService.GetUserPreferencesAsync(command.UserId);

            // algoritmo de matching 
            var matches = recipes
                .Select(recipe => CalculateMatch(recipe, sortedIngredients))
                .Where(match => match.MatchPercentage >= MinMatchPercentage)
                //.Where(match => !ViolatesPreferences(match, recipes, preferences))
                .OrderByDescending(match => match.MatchPercentage)
                .ToList();

            // cachear 
            await _cache.SetAsync(cacheKey, matches, CacheDuration);

            return matches;
        }

        private static IngredientMatchDto CalculateMatch(RecipeDto recipe, List<string> requestedIngredients)
        {
            var recipeIngredients = recipe.Ingredients
                .Select(i => i.Name.ToLowerInvariant().Trim())
                .ToList();

            var matched = requestedIngredients
                .Intersect(recipeIngredients)
                .ToList();

            var missing = recipeIngredients
                .Except(requestedIngredients)
                .ToList();

            // matchPercentage = (ingredientes_coincidentes / total_ingredientes_receta) * 100 

            var matchPercentage = recipeIngredients.Count > 0
                ? (decimal)matched.Count / recipeIngredients.Count * 100
                : 0;

            return new IngredientMatchDto
            {
                RecipeId = recipe.recipe.Id,
                Title = recipe.recipe.Title,
                MatchPercentage = Math.Round(matchPercentage, 2),
                MatchedIngredients = matched.ToList(),
                MissingIngredients = missing.ToList()
            };
        }

        //private static bool ViolatesPreferences(
        //    IngredientMatchDto match,
        //    List<RecipeDto> recipes, 
        //    UserPreferencesDto? preferences)
        //{
        //    if (preferences is null) return false;

        //    var recipe = recipes.FirstOrDefault(r => r.recipe.Id == match.RecipeId);
        //    if (recipe is null) return false; 

        //    // verificar restricciones dietéticas 
        //    if (preferences)
        //}

        private static string ComputeHash(List<string> ingredients)
        {
            var input = string.Join(",", ingredients);
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes)[..16]; // solo los primeros 16 chars 

        }
    }
}
