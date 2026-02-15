using BuildingBlocks.CQRS;
using Mapster;
using Recipe.API.Features.Recipe.CreateRecipe;
using Recipe.API.Models;
using Recipe.API.Repositories.RecipeRepository;

namespace Recipe.API.Features.Recipe.SearchAdvancedRecipe
{
    public class SearchAdvancedRecipeQueryHandler : IQueryHandler<SearchAdvancedRecipeQuery, SearchAdvancedRecipeResponse>
    {
        private readonly IRecipeRepository _recipeRepository;

        public SearchAdvancedRecipeQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }
        public async Task<SearchAdvancedRecipeResponse> Handle(SearchAdvancedRecipeQuery query, CancellationToken cancellationToken)
        {
            var criteria = query.Adapt<RecipeSearchCriteria>(); 
            var recipes = await _recipeRepository.SearchAdvanced(criteria);

            var mapRecipes = recipes.Adapt<List<ResponseRecipe>>().ToList();

            var result = new SearchAdvancedRecipeResponse(mapRecipes);

            return result; 
        }
    }
}
