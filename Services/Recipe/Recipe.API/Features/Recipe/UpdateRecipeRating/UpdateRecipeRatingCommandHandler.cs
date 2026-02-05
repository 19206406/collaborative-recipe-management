using BuildingBlocks.CQRS;
using Recipe.API.Repositories.RepositoryInterfaces;

namespace Recipe.API.Features.Recipe.UpdateRecipeRating
{
    public class UpdateRecipeRatingCommandHandler : ICommandHandler<UpdateRecipeRatingCommand, UpdateRecipeRatingResponse>
    {
        private readonly IRecipeRepository _recipeRepository;

        public UpdateRecipeRatingCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public Task<UpdateRecipeRatingResponse> Handle(UpdateRecipeRatingCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
