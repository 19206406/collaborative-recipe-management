using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Mapster;
using Rating.API.Entities;
using Rating.API.Repositories;

namespace Rating.API.Features.CreateAndUpdateRating
{
    public class CreateAndUpdateRatingCommandHandler : ICommandHandler<CreateAndUpdateRatingCommand, CreateAndUpdateRatingResponse>
    {
        private readonly IRatingRepository _ratingRepository;

        public CreateAndUpdateRatingCommandHandler(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<CreateAndUpdateRatingResponse> Handle(CreateAndUpdateRatingCommand command, CancellationToken cancellationToken)
        {
            if (command.IsToUpdate)
            {
                var rating = await _ratingRepository.GetRating(command.Id);

                if (rating is null)
                    throw new NotFoundException("calificación", command.Id);

                if (!string.IsNullOrEmpty(command.Comment))
                    rating.Comment = command.Comment; 
                    

                rating.Rating = command.Rating;
                rating.UpdatedAt = DateTime.UtcNow;

                var updatedRating = await _ratingRepository.UpdateRating(rating);

                return updatedRating.Adapt<CreateAndUpdateRatingResponse>(); 
            }
            else
            {
                var newRating = new RatingE
                {
                    UserId = command.UserId, 
                    RecipeId = command.RecipeId, 
                    Rating = command.Rating, 
                    Comment = command.Comment, 
                    CreatedAt = DateTime.UtcNow, 
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await _ratingRepository.AddRating(newRating);

                return result.Adapt<CreateAndUpdateRatingResponse>();
            }
        }
    }
}
