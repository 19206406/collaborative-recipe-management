using BuildingBlocks.CQRS;
using Recommendation.API.Common.Dtos;

namespace Recommendation.API.Features.Recommendation.GetPersonalizedRecipes
{
    public record GetPersonalizedRecipesQuery(int UserId) : IQuery<List<RecipeDto>>; 
}
