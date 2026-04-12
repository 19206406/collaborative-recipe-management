using BuildingBlocks.CQRS;
using Recommendation.API.Common.Cache;
using Recommendation.API.Common.Dtos;
using Recommendation.API.Features.Clients.RecipeClient;
using System.Security.Cryptography;
using System.Text;

namespace Recommendation.API.Features.Recommendation.RecommendationsByIngredients
{
    public class RecommendationsByIngredientsCommandHandler
        : ICommandHandler<RecommendationsByIngredientsCommand, List<IngredientMatchDto>>
    {
        private readonly IRecipeServiceClient _recipeService;
        private readonly ICacheService _cache;

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);
        private const decimal MinMatchPercentage = 60m;

        public RecommendationsByIngredientsCommandHandler(
            IRecipeServiceClient recipeService, ICacheService cache)
        {
            _recipeService = recipeService;
            _cache = cache;
        }

        public async Task<List<IngredientMatchDto>> Handle(RecommendationsByIngredientsCommand command, CancellationToken cancellationToken)
        {
            var sortedIngredients = command.Ingredients
                .Select(i => i.ToLowerInvariant().Trim())
                .OrderBy(i => i)
                .ToList();

            var hash = ComputeHash(sortedIngredients);
            var cacheKey = command.UserId != 0
                ? $"ingredients:{hash}:user:{command.UserId}"
                : $"ingredients:{hash}"; 

            var cached = await _cache.GetAsync<List<IngredientMatchDto>>(cacheKey);
            if (cached is not null)
                return cached;

            var recipes = await _recipeService.GetByIngredientsAsync(sortedIngredients);

            var matches = recipes
                .Select(recipe => CalculateMatch(recipe, sortedIngredients))
                .Where(match => match.MatchPercentage >= MinMatchPercentage)
                .OrderByDescending(match => match.MatchPercentage)
                .ToList(); 

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

            var matchPercentage = recipeIngredients.Count > 0
                ? (decimal)matched.Count / recipeIngredients.Count * 100
                : 0;

            return new IngredientMatchDto
            {
                RecipeId = recipe.Id,
                Title = recipe.Title,
                MatchPercentage = Math.Round(matchPercentage, 2),
                MatchedIngredients = matched.ToList(),
                MissingIngredients = missing.ToList()
            };
        }

        private static string ComputeHash(List<string> ingredients)
        {
            var input = string.Join(",", ingredients);
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes)[..16];
        }
    }
}
