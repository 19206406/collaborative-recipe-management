using BuildingBlocks.CQRS;
using Mapster;
using Recipe.API.Features.Recipe.CreateRecipe;
using Recipe.API.Repositories.RecipeRepository;

namespace Recipe.API.Features.Recipe.GetRecipesByIngredients
{
    public class GetRecipesByIngredientsQueryHandler 
        : IQueryHandler<GetRecipesByIngredientsQuery, GetRecipesByIngredientsResponse>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetRecipesByIngredientsQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<GetRecipesByIngredientsResponse> Handle(GetRecipesByIngredientsQuery query, CancellationToken cancellationToken)
        {
            var recipes = await _recipeRepository.GetRecipesByIngredientsAsync(query.Ingredients);
            var map = recipes.Adapt<List<ResponseRecipe>>(); 

            return new GetRecipesByIngredientsResponse(map); 
        }
    }
}
