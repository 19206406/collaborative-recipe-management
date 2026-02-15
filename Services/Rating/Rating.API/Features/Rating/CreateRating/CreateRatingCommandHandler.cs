using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Mapster;
using Rating.API.Entities;
using Rating.API.Features.Clients;
using Rating.API.Repositories;

namespace Rating.API.Features.Rating.CreateRating
{
    public class CreateRatingCommandHandler : ICommandHandler<CreateRatingCommand, CreateRatingResponse>
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IRecipesServiceClient _recipesClient;

        public CreateRatingCommandHandler(IRatingRepository ratingRepository, IRecipesServiceClient recipesClient)
        {
            _ratingRepository = ratingRepository;
            _recipesClient = recipesClient;
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

            bool recipeExist = await _recipesClient.RecipeExistAsync(command.RecipeId, cancellationToken);

            if (!recipeExist)
                throw new NotFoundException("receta", command.RecipeId); 

            var createdRating = await _ratingRepository.AddRating(newRating);   

            return createdRating.Adapt<CreateRatingResponse>(); 
        }
    }
}
