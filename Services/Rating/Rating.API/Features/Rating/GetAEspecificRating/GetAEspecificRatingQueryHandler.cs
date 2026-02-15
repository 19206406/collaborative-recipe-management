using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Rating.API.Features.Clients;
using Rating.API.Repositories;

namespace Rating.API.Features.Rating.GetAEspecificRating
{
    public class GetAEspecificRatingQueryHandler : IQueryHandler<GetAEspecificRatingQuery, GetAEspecificRatingResponse>
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IRecipesServiceClient _recipesClient;

        public GetAEspecificRatingQueryHandler(IRatingRepository ratingRepository, IRecipesServiceClient recipesClient)
        {
            _ratingRepository = ratingRepository;
            _recipesClient = recipesClient;
        }

        public async Task<GetAEspecificRatingResponse> Handle(GetAEspecificRatingQuery query, CancellationToken cancellationToken)
        {
            //TODO : Implementar si un usuario existe 

            bool recipeExist = await _recipesClient.RecipeExistAsync(query.RecipeId, cancellationToken);

            if (!recipeExist)
                throw new NotFoundException("receta", query.RecipeId);

            var rating = await _ratingRepository.GetSpecificRatingAsync(query.UserId, query.RecipeId);

            return new GetAEspecificRatingResponse(query.UserId, query.RecipeId, rating); 
        }
    }
}
