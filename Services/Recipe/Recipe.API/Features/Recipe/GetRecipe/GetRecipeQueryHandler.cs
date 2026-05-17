using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Mapster;
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
            var recipe = await _recipeRepository.GetRecipe(query.Id);

            if (recipe is null)
                throw new NotFoundException("receta", query.Id);

            var ingredients = recipe.Ingredients.Adapt<List<ResponseIngredient>>().ToList();
            var steps = recipe.Steps.Adapt<List<ResponseStep>>().ToList();
            var tags = recipe.RecipeTags.Adapt<List<ResponseTag>>().ToList();

            var mapRecipe = new GetRecipeResponse(recipe.Id, recipe.UserId, recipe.Title, recipe.Description, recipe.PrepTimeMinutes, 
                recipe.CookTimeMinutes, recipe.Difficulty, recipe.Servings, recipe.ImageUrl, recipe.AverageRating, recipe.RatingCount, 
                recipe.CreatedAt, recipe.UpdatedAt, ingredients, steps, tags); 

            return mapRecipe;
        }
    }
}
