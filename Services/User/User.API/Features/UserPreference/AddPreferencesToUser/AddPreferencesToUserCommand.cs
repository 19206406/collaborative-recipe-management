using BuildingBlocks.CQRS;

namespace User.API.Features.UserPreference.AddPreferencesToUser
{
    public record AddPreferencesToUserCommand(int Id, List<Entities.UserPreference> UserPreferences) 
        : ICommand<AddPreferencesToUserResponse>; 
}
