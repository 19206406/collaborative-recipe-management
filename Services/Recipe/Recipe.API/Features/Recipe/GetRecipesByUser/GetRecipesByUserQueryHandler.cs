using BuildingBlocks.CQRS;
using Mapster;
using Recipe.API.Features.Recipe.CreateRecipe;
using Recipe.API.Repositories.RecipeRepository;

namespace Recipe.API.Features.Recipe.GetRecipesByUser
{
    public class GetRecipesByUserQueryHandler : IQueryHandler<GetRecipesByUserQuery, GetRecipesByUserResponse>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetRecipesByUserQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<GetRecipesByUserResponse> Handle(GetRecipesByUserQuery query, CancellationToken cancellationToken)
        {
            var recipes = await _recipeRepository.GetRecipesByUser(query.UserId);

            var mapRecipes = recipes.Adapt<List<ResponseRecipe>>().ToList();

            return new GetRecipesByUserResponse(mapRecipes); 
        }
    }
}
