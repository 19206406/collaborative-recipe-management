using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using MediatR;
using Rating.API.Repositories;

namespace Rating.API.Features.RemoveRating
{
    public class RemoveRatingCommandHandler : ICommandHandler<RemoveRatingCommand, Unit>
    {
        private readonly IRatingRepository _ratingRepository;

        public RemoveRatingCommandHandler(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<Unit> Handle(RemoveRatingCommand command, CancellationToken cancellationToken)
        {
            var rating = await _ratingRepository.GetRating(command.Id);

            if (rating is null)
                throw new NotFoundException("calificación", command.Id);

            await _ratingRepository.DeleteRatingAsync(rating);

            return Unit.Value; 
        }
    }
}
