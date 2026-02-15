using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Mapster;
using Recipe.API.Features.Recipe.CreateRecipe;
using Recipe.API.Repositories.RecipeRepository;

namespace Recipe.API.Features.Recipe.GetOnlyRecipe
{
    public class GetOnlyRecipeQueryHandler : IQueryHandler<GetOnlyRecipeQuery, GetOnlyRecipeResponse>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetOnlyRecipeQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<GetOnlyRecipeResponse> Handle(GetOnlyRecipeQuery query, CancellationToken cancellationToken)
        {
            var recipe = await _recipeRepository.GetRecipe(query.Id);

            if (recipe is null)
                throw new NotFoundException("receta", query.Id);

            var response = new GetOnlyRecipeResponse(recipe.Adapt<ResponseRecipe>());
            return response; 
        }
    }
}
