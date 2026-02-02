using BuildingBlocks.CQRS;
using Recipe.API.Repositories;

namespace Recipe.API.Features.Recipe.GetRecipe
{
    public class GetRecipeQueryHandler : IQueryHandler<GetRecipeQuery, GetRecipeResponse>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetRecipeQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public Task<GetRecipeResponse> Handle(GetRecipeQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
