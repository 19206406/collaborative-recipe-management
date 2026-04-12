using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using MediatR;
using Rating.API.Features.Clients;
using Rating.API.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Rating.API.Features.Rating.RemoveRating
{
    public class RemoveRatingCommandHandler : ICommandHandler<RemoveRatingCommand, RemoveRatingResponse>
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IRecipesServiceClient _recipesClient;

        public RemoveRatingCommandHandler(IRatingRepository ratingRepository, IRecipesServiceClient recipesClient)
        {
            _ratingRepository = ratingRepository;
            _recipesClient = recipesClient;
        }

        public async Task<RemoveRatingResponse> Handle(RemoveRatingCommand command, CancellationToken cancellationToken)
        {
            var rating = await _ratingRepository.GetRating(command.Id);

            if (rating is null)
                throw new NotFoundException("calificación", command.Id);

            if (command.UserId != rating.UserId)
                throw new UnauthorizedException("El usuario no está autorizado para realizar esta acción");

            await _ratingRepository.DeleteRatingAsync(rating);

            return new RemoveRatingResponse(rating.Id, rating.UserId, rating.RecipeId, rating.Rating, rating.Comment); 
        }
    }
}
