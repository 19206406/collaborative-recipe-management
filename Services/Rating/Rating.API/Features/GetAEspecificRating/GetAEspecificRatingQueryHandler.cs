using BuildingBlocks.CQRS;
using Rating.API.Repositories;

namespace Rating.API.Features.GetAEspecificRating
{
    public class GetAEspecificRatingQueryHandler : IQueryHandler<GetAEspecificRatingQuery, GetAEspecificRatingResponse>
    {
        private readonly IRatingRepository _ratingRepository;

        public GetAEspecificRatingQueryHandler(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<GetAEspecificRatingResponse> Handle(GetAEspecificRatingQuery query, CancellationToken cancellationToken)
        {
            //TODO : Implementar si un usuario existe 
            //TODO : Implementar si una receta existe 

            var rating = await _ratingRepository.GetSpecificRatingAsync(query.UserId, query.RecipeId);

            return new GetAEspecificRatingResponse(query.UserId, query.RecipeId, rating); 
        }
    }
}
