using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Mapster;
using Rating.API.Features.Clients;
using Rating.API.Repositories;

namespace Rating.API.Features.Rating.GetRecipeRatings
{
    public class GetRecipeRatingsQueryHandler : IQueryHandler<GetRecipeRatingsQuery, GetRecipeRatingsResponse>
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IRecipesServiceClient _recipesClient;

        public GetRecipeRatingsQueryHandler(IRatingRepository ratingRepository, IRecipesServiceClient recipesClient)
        {
            _ratingRepository = ratingRepository;
            _recipesClient = recipesClient;
        }

        public async Task<GetRecipeRatingsResponse> Handle(GetRecipeRatingsQuery query, CancellationToken cancellationToken)
        {
            //bool recipeExist = await _recipesClient.RecipeExistAsync(query.RecipeId, cancellationToken);

            //if (!recipeExist)
            //    throw new NotFoundException("receta", query.RecipeId);

            var ratings = await _ratingRepository.GetRatingsByRecipeIdAsync(query.RecipeId);

            var ratingsMap = ratings.Adapt<List<RatingResponse>>();

            return new GetRecipeRatingsResponse(ratingsMap); 
        }
    }
}
