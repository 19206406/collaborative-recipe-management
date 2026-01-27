using BuildingBlocks.CQRS;

namespace User.API.Features.UserPreference.UpdatePreferencesToUser
{
    public record UpdatePreferencesToUserCommand(int UserId, List<Entities.UserPreference> UserPreferences) 
        : ICommand<UpdatePreferencesToUserResponse>; 
}
