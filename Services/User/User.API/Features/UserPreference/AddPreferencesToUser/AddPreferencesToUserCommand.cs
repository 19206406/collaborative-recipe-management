using BuildingBlocks.CQRS;

namespace User.API.Features.UserPreference.AddPreferencesToUser
{
    public record AddPreferencesToUserCommand(int Id, List<string> preferences) 
        : ICommand<AddPreferencesToUserResponse>; 
}
