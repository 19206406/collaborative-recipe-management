using BuildingBlocks.CQRS;
using Mapster;
using MediatR;
using Recipe.API.Features.Recipe.CreateRecipe;
using Recipe.API.Repositories.RecipeRepository;

namespace Recipe.API.Features.Recipe.GetTopRecipes
{
    public class GetTopRecipesQueryHandler : IQueryHandler<GetTopRecipesQuery, GetTopRecipesResponse>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetTopRecipesQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<GetTopRecipesResponse> Handle(GetTopRecipesQuery request, CancellationToken cancellationToken)
        {
            var recipes = await _recipeRepository.GetTopRecipesAsync();
            var mapRecipes = recipes.Adapt<List<ResponseRecipe>>(); 

            return new GetTopRecipesResponse(mapRecipes); 
        }
    }
}
