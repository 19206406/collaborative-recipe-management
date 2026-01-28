using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using User.API.repositories.UserPreferenceRepository;

namespace User.API.Features.UserPreference.UpdatePreferencesToUser
{
    public class UpdatePreferencesToUserCommandHandler 
        : ICommandHandler<UpdatePreferencesToUserCommand, UpdatePreferencesToUserResponse>
    {
        private readonly IUserPreferenceRespository _userPreferenceRespository;

        public UpdatePreferencesToUserCommandHandler(IUserPreferenceRespository userPreferenceRespository)
        {
            _userPreferenceRespository = userPreferenceRespository;
        }

        public async Task<UpdatePreferencesToUserResponse> Handle(UpdatePreferencesToUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userPreferenceRespository.GetUserPreferences(command.UserId);

            if (user is null)
                throw new NotFoundException("usuario", command.UserId); 

            var userPreferences = user.UserPreferences.ToList();
            var updatePreferences = command.UserPreferences;

            // beging logic 
            // delete preferences that are no longer there 
            var preferencesDelete = userPreferences
                .Where(exist => !updatePreferences.Any(np => np.Id == exist.Id)).ToList();

            await _userPreferenceRespository.RemoveReferences(preferencesDelete);

            // update existing ones 
            var preferencesToUpdate = userPreferences
                .Where(exist => updatePreferences.Any(np => np.Id == exist.Id)).ToList();

            await _userPreferenceRespository.UpdateReferences(preferencesToUpdate);

            // add preferences 

            var newPreferences = updatePreferences
                .Where(up => up.Id == 0 || !userPreferences.Any(upr => upr.Id == up.Id))
                .ToList();

            await _userPreferenceRespository.AddUserPreferences(command.UserId, newPreferences);

            var userWithPreferences = await _userPreferenceRespository.GetUserPreferences(command.UserId);

            return new UpdatePreferencesToUserResponse(userWithPreferences); 
        }
    }
}
