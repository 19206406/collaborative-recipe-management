using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using User.API.Features.UserPreference.GetUserPreferences;
using User.API.repositories.UserPreferenceRepository;
using User.API.repositories.UserRespository;

namespace User.API.Features.UserPreference.AddPreferencesToUser
{
    public class AddPreferencesToUserCommandHandler 
        : ICommandHandler<AddPreferencesToUserCommand, AddPreferencesToUserResponse>
    {
        private readonly IUserPreferenceRespository _userPreferenceRespository;

        public AddPreferencesToUserCommandHandler
            (IUserPreferenceRespository userPreferenceRespository, IUserRepository userRepository)
        {
            _userPreferenceRespository = userPreferenceRespository;
        }

        public async Task<AddPreferencesToUserResponse> Handle(AddPreferencesToUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userPreferenceRespository.GetUserPreferences(command.Id);

            if (user is null)
                throw new NotFoundException("usuario", command.Id);

            var newPreferences = command.preferences.ToList();
            var result = await _userPreferenceRespository.AddUserPreferences(command.Id, newPreferences);

            var updatedUser = await _userPreferenceRespository.GetUserPreferences(command.Id);

            var preferences = updatedUser.UserPreferences.Select(x => new PreferencesResponse(x.Id, x.PreferenceType)).ToList();

            return new AddPreferencesToUserResponse(updatedUser.Id, updatedUser.Name, 
                updatedUser.Email, updatedUser.CreatedAt, preferences);  
        }
    }
}
