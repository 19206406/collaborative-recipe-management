using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Rating.API.Entities;
using Rating.API.Features.Clients;
using Rating.API.Repositories;
using InvalidOperationException = BuildingBlocks.Exceptions.InvalidOperationException;

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
            bool recipeExist = await _recipesClient.RecipeExistAsync(command.RecipeId, cancellationToken);
            if (!recipeExist)
                throw new NotFoundException("receta", command.RecipeId);

            if (command.IsToUpdate)
            {
                var rating = await _ratingRepository.GetRating(command.Id);
                int oldRating = rating.Rating; 
                if (rating is null)
                    throw new NotFoundException("calificación", command.Id);

                if (command.UserId != rating.UserId)
                    throw new UnauthorizedException("El usuario no puede ejecutar esta acción en este elemento en especifico"); 


                rating.Rating = command.Rating;
                rating.Comment = command.Comment; 
                rating.UpdatedAt = DateTime.UtcNow;

                var updatedRating = await _ratingRepository.UpdateRating(rating);

                return new CreateAndUpdateRatingResponse(rating.Id, rating.UserId, rating.RecipeId, rating.Rating, oldRating, 
                    rating.Comment, rating.CreatedAt, rating.UpdatedAt);
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

                var rating = await _ratingRepository.GetSpecificRatingAsync(command.UserId, command.RecipeId);

                if (rating is not null)
                    throw new InvalidOperationException("No puedes ejecutar esta acción de nuevo"); 

                var result = await _ratingRepository.AddRating(newRating);

                return new CreateAndUpdateRatingResponse(result.Id, result.UserId, result.RecipeId, result.Rating, 0, result.Comment, result.CreatedAt, result.UpdatedAt);
            }
        }
    }
}
