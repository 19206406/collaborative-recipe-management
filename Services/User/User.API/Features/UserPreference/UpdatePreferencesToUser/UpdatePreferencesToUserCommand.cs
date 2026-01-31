using BuildingBlocks.CQRS;

namespace User.API.Features.UserPreference.UpdatePreferencesToUser
{
    public record UpdatePreferencesToUserCommand(int UserId, List<UpdatePreferences> Preferences) 
        : ICommand<UpdatePreferencesToUserResponse>; 
}
