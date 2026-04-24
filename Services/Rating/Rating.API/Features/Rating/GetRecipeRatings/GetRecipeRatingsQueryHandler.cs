using BuildingBlocks.CQRS;
using Mapster;
using Rating.API.Features.Clients.RecipeClient;
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
            var ratings = await _ratingRepository.GetRatingsByRecipeIdAsync(query.RecipeId);

            var ratingsMap = ratings.Adapt<List<RatingResponse>>();

            return new GetRecipeRatingsResponse(ratingsMap); 
        }
    }
}
