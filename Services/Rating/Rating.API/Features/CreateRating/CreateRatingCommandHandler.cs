using BuildingBlocks.CQRS;
using Mapster;
using Rating.API.Entities;
using Rating.API.Repositories;

namespace Rating.API.Features.CreateRating
{
    public class CreateRatingCommandHandler : ICommandHandler<CreateRatingCommand, CreateRatingResponse>
    {
        private readonly IRatingRepository _ratingRepository;

        public CreateRatingCommandHandler(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<CreateRatingResponse> Handle(CreateRatingCommand command, CancellationToken cancellationToken)
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

            var createdRating = await _ratingRepository.AddRating(newRating);   

            return createdRating.Adapt<CreateRatingResponse>(); 
        }
    }
}
