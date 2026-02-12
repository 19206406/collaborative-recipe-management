using BuildingBlocks.CQRS;
using Rating.API.Repositories;

namespace Rating.API.Features.GetAverageRating
{
    public class GetAverageRatingQueryHandler : IQueryHandler<GetAverageRatingQuery, GetAverageRatingResponse>
    {
        private readonly IRatingRepository _ratingRepository;

        public GetAverageRatingQueryHandler(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<GetAverageRatingResponse> Handle(GetAverageRatingQuery query, CancellationToken cancellationToken)
        {
            // validar si existe una receta con el id proporcionado 

            var averageRating = await _ratingRepository.GetAverageRatingAsync(query.RecipeId);

            return new GetAverageRatingResponse(query.RecipeId, averageRating); 
        }
    }
}
