using BuildingBlocks.CQRS;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Recipe.API.Features.Recipe.CreateRecipe;
using BuildingBlocks.Pagination;
using Recipe.API.Repositories.RepositoryInterfaces;

namespace Recipe.API.Features.Recipe.GetListOfRecipes
{
    public class GetListOfRecipesQueryHandler : IQueryHandler<GetListOfRecipesQuery, GetListOfRecipesResponse>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetListOfRecipesQueryHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<GetListOfRecipesResponse> Handle(GetListOfRecipesQuery query, CancellationToken cancellationToken)
        {
            var pageNumber = query.PageNumber;
            var pageSize = query.PageSize;

            var totalCount = await _recipeRepository.NumberOfItems();

            var recipes = await _recipeRepository.GetRecipePagination(pageNumber, pageSize);

            var mapRecipes = recipes.Adapt<List<ResponseRecipe>>().ToList();

            return new GetListOfRecipesResponse(
                new PaginatedResult<ResponseRecipe>(pageNumber, pageSize, totalCount, mapRecipes)); 
        }
    }
}
