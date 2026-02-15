using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Mapster;
using Recipe.API.Features.Recipe.CreateRecipe;
using Recipe.API.Repositories.RecipeRepository;

namespace Recipe.API.Features.Recipe.GetRecipe
{
    public class GetRecipeQueryHandler : IQueryHandler<GetRecipeQuery, GetRecipeResponse>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetRecipeQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<GetRecipeResponse> Handle(GetRecipeQuery query, CancellationToken cancellationToken)
        {
            var r = await _recipeRepository.GetRecipe(query.Id);

            if (r is null)
                throw new NotFoundException("receta", query.Id);

            var recipe = r.Adapt<ResponseRecipe>();
            var ingredients = r.Adapt<List<ResponseIngredient>>().ToList();
            var steps = r.Adapt<List<ResponseStep>>().ToList();
            var tags = r.Adapt<List<ResponseTag>>().ToList(); 

            return new GetRecipeResponse(recipe, ingredients, steps, tags); 
        }
    }
}
