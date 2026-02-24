using BuildingBlocks.CQRS;
using Recommendation.API.Common.Dtos;

namespace Recommendation.API.Features.Recommendation.GetSimilarRecipes
{
    public record GetSimilarRecipesQuery(int recipeId) : IQuery<List<RecipeDto>>; 
}
