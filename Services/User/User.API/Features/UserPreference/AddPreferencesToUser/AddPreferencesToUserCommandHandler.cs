using BuildingBlocks.CQRS;
using User.API.repositories.UserPreferenceRepository;

namespace User.API.Features.UserPreference.AddPreferencesToUser
{
    public class AddPreferencesToUserCommandHandler 
        : ICommandHandler<AddPreferencesToUserCommand, AddPreferencesToUserResponse>
    {
        private readonly IUserPreferenceRespository _userPreferenceRespository;

        public AddPreferencesToUserCommandHandler(IUserPreferenceRespository userPreferenceRespository)
        {
            _userPreferenceRespository = userPreferenceRespository;
        }

        public async Task<AddPreferencesToUserResponse> Handle(AddPreferencesToUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userPreferenceRespository.GetUserPreferences(command.Id);

            if (user is null)
                throw new Exception();

            var newPreferences = command.UserPreferences.ToList();
            var result = await _userPreferenceRespository.AddUserPreferences(command.Id, newPreferences);

            if (!result)
                throw new Exception();

            var updatedUser = await _userPreferenceRespository.GetUserPreferences(command.Id);

            return new AddPreferencesToUserResponse(updatedUser); 
        }
    }
}
