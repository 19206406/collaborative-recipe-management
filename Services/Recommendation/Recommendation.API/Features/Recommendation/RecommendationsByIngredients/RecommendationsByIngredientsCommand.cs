using BuildingBlocks.CQRS;
using Recommendation.API.Common.Dtos;

namespace Recommendation.API.Features.Recommendation.RecommendationsByIngredients
{
    public record RecommendationsByIngredientsCommand(int UserId, List<string> Ingredients) : ICommand<List<IngredientMatchDto>>; 
}
