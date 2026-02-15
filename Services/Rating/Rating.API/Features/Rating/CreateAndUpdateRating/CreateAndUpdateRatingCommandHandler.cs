using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Mapster;
using Rating.API.Entities;
using Rating.API.Features.Clients;
using Rating.API.Repositories;

namespace Rating.API.Features.Rating.CreateAndUpdateRating
{
    public class CreateAndUpdateRatingCommandHandler : ICommandHandler<CreateAndUpdateRatingCommand, CreateAndUpdateRatingResponse>
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IRecipesServiceClient _recipesClient;

        public CreateAndUpdateRatingCommandHandler(IRatingRepository ratingRepository, IRecipesServiceClient recipesClient)
        {
            _ratingRepository = ratingRepository;
            _recipesClient = recipesClient;
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

                bool recipeExist = await _recipesClient.RecipeExistAsync(command.RecipeId, cancellationToken);

                if (!recipeExist)
                    throw new NotFoundException("receta", command.RecipeId);

                var result = await _ratingRepository.AddRating(newRating);

                return result.Adapt<CreateAndUpdateRatingResponse>();
            }
        }
    }
}
