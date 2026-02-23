using Recommendation.API.Common.Dtos;

namespace Recommendation.API.Features.Recommendation.RecommendationsByIngredients
{
    public record RecommendationsByIngredientsResponse(List<IngredientMatchDto> IngredientMatches); 
}
