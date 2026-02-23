//using MediatR;
//using Recommendation.API.Common.Cache;
//using Recommendation.API.Common.Dtos;
//using Recommendation.API.Common.DTOs;
//using Recommendation.API.Common.Services;
//using Recommendation.API.Features.Clients.RecipeClient;
//using Recommendation.API.Features.Clients.UserClient;
//using System.Security.Cryptography;
//using System.Text;

//namespace Recommendation.API.Features.ByIngredients;

//public class ByIngredientsHandler(
//    IRecipeServiceClient recipeService,
//    IUserServiceClient userService,
//    ICacheService cache
//) : IRequestHandler<ByIngredientsQuery, List<IngredientMatchDto>>
//{
//    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);
//    private const decimal MinMatchPercentage = 60m;

//    public async Task<List<IngredientMatchDto>> Handle(
//        ByIngredientsQuery request, CancellationToken cancellationToken)
//    {
//        // Generamos cache key única basada en los ingredientes (ordenados para consistencia)
//        var sortedIngredients = request.Ingredients
//            .Select(i => i.ToLowerInvariant().Trim())
//            .OrderBy(i => i)
//            .ToList();

//        var hash = ComputeHash(sortedIngredients);
//        var cacheKey = $"ingredients:{hash}";

//        // Intentamos obtener del cache primero
//        var cached = await cache.GetAsync<List<IngredientMatchDto>>(cacheKey);
//        if (cached is not null)
//            return cached;

//        // Llamamos en paralelo a Recipe Service y User Service
//        var recipesTask = recipeService.GetByIngredientsAsync(sortedIngredients);
//        var preferencesTask = request.UserId.HasValue
//            ? userService.GetPreferencesAsync(request.UserId.Value)
//            : Task.FromResult<UserPreferencesDto?>(null);

//        await Task.WhenAll(recipesTask, preferencesTask);

//        var recipes = await recipesTask;
//        var preferences = await preferencesTask;

//        // Algoritmo de matching
//        var matches = recipes
//            .Select(recipe => CalculateMatch(recipe, sortedIngredients))
//            .Where(match => match.MatchPercentage >= MinMatchPercentage)
//            .Where(match => !ViolatesPreferences(match, recipes, preferences))
//            .OrderByDescending(match => match.MatchPercentage)
//            .ToList();

//        // Cacheamos el resultado
//        await cache.SetAsync(cacheKey, matches, CacheDuration);

//        return matches;
//    }

//    private static IngredientMatchDto CalculateMatch(RecipeDto recipe, List<string> requestedIngredients)
//    {
//        var recipeIngredients = recipe.Ingredients
//            .Select(i => i.ToLowerInvariant().Trim())
//            .ToList();

//        var matched = requestedIngredients
//            .Intersect(recipeIngredients)
//            .ToList();

//        var missing = recipeIngredients
//            .Except(requestedIngredients)
//            .ToList();

//        // matchPercentage = (ingredientes_coincidentes / total_ingredientes_receta) * 100
//        var matchPercentage = recipeIngredients.Count > 0
//            ? (decimal)matched.Count / recipeIngredients.Count * 100
//            : 0;

//        return new IngredientMatchDto
//        {
//            RecipeId = recipe.Id,
//            Title = recipe.Title,
//            MatchPercentage = Math.Round(matchPercentage, 2),
//            MatchedIngredients = matched,
//            MissingIngredients = missing
//        };
//    }

//    private static bool ViolatesPreferences(
//        IngredientMatchDto match,
//        List<RecipeDto> recipes,
//        UserPreferencesDto? preferences)
//    {
//        if (preferences is null) return false;

//        var recipe = recipes.FirstOrDefault(r => r.Id == match.RecipeId);
//        if (recipe is null) return false;

//        // Verificamos restricciones dietéticas (ej: si es vegetariano, excluir recetas con carne)
//        if (preferences.DietaryRestrictions.Contains("vegetarian", StringComparer.OrdinalIgnoreCase))
//        {
//            var meatIngredients = new[] { "pollo", "carne", "cerdo", "res", "chicken", "beef", "pork" };
//            if (recipe.Ingredients.Any(i => meatIngredients.Any(meat =>
//                i.Contains(meat, StringComparison.OrdinalIgnoreCase))))
//                return true;
//        }

//        // Verificamos ingredientes excluidos por el usuario
//        if (preferences.ExcludedIngredients.Any(excluded =>
//            recipe.Ingredients.Any(i => i.Contains(excluded, StringComparison.OrdinalIgnoreCase))))
//            return true;

//        return false;
//    }

//    private static string ComputeHash(List<string> ingredients)
//    {
//        var input = string.Join(",", ingredients);
//        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
//        return Convert.ToHexString(bytes)[..16]; // Solo los primeros 16 chars son suficientes
//    }
//}