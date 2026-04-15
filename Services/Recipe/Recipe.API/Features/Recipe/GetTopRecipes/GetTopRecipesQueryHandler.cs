using BuildingBlocks.CQRS;
using Mapster;
using Recipe.API.Repositories.RecipeRepository;

namespace Recipe.API.Features.Recipe.GetTopRecipes
{
    public class GetTopRecipesQueryHandler : IQueryHandler<GetTopRecipesQuery, List<GetTopRecipesResponse>>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetTopRecipesQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<List<GetTopRecipesResponse>> Handle(GetTopRecipesQuery request, CancellationToken cancellationToken)
        {
            var recipes = await _recipeRepository.GetTopRecipesAsync();
            var mapRecipes = recipes.Adapt<List<GetTopRecipesResponse>>();

            return mapRecipes; 
        }
    }
}
