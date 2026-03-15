using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Rating.API.Features.Clients;
using Rating.API.Repositories;

namespace Rating.API.Features.Rating.GetAverageRating
{
    public class GetAverageRatingQueryHandler : IQueryHandler<GetAverageRatingQuery, GetAverageRatingResponse>
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IRecipesServiceClient _recipesClient;

        public GetAverageRatingQueryHandler(IRatingRepository ratingRepository, IRecipesServiceClient recipesClient)
        {
            _ratingRepository = ratingRepository;
            _recipesClient = recipesClient;
        }

        public async Task<GetAverageRatingResponse> Handle(GetAverageRatingQuery query, CancellationToken cancellationToken)
        {
            //bool recipeExist = await _recipesClient.RecipeExistAsync(query.RecipeId, cancellationToken);

            //if (!recipeExist)
            //    throw new NotFoundException("receta", query.RecipeId);

            var averageRating = await _ratingRepository.GetAverageRatingAsync(query.RecipeId);

            return new GetAverageRatingResponse(query.RecipeId, averageRating); 
        }
    }
}
