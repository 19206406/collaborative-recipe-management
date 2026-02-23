using BuildingBlocks.CQRS;
using Recommendation.API.Common.Dtos;

namespace Recommendation.API.Features.Recommendation.GetTrendingRecipes
{
    public record GetTrendingRecipesQuery() : IQuery<List<RecipeDto>>; 
}
