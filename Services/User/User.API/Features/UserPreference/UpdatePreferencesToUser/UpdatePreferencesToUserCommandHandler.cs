using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using User.API.Features.UserPreference.GetUserPreferences;
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
            var updatePreferences = command.Preferences;

            // remover las preferencias no existentes 
            var preferencesToDelete = userPreferences
                .Where(exist => !updatePreferences.Any(np => np.Id == exist.Id)).ToList();
            await _userPreferenceRespository.RemoveReferences(preferencesToDelete);


            // update existing ones 
            var preferencesToUpdate = userPreferences
                .Where(exist => updatePreferences.Any(np => np.Id == exist.Id)).ToList();

            foreach (var preference in preferencesToUpdate)
            {
                var map = updatePreferences.First(p => p.Id == preference.Id);
                preference.PreferenceType = map.Preference; 
            }

            await _userPreferenceRespository.UpdateReferences(preferencesToUpdate);


            // add preferences 
            var newPreferences = updatePreferences
                .Where(up => up.Id == 0 || !userPreferences.Any(upr => upr.Id == up.Id))
                .ToList();
            List<string> preferences = newPreferences.Select(p => p.Preference.ToString()).ToList(); 
            await _userPreferenceRespository.AddUserPreferences(command.UserId, preferences);


            var uwp = await _userPreferenceRespository.GetUserPreferences(command.UserId);
            var resultPreferences = user.UserPreferences.Select(x => new PreferencesResponse(x.Id, x.PreferenceType)).ToList();
            return new UpdatePreferencesToUserResponse(uwp.Id, uwp.Name, uwp.Email, uwp.CreatedAt, resultPreferences); 
        }
    }
}
