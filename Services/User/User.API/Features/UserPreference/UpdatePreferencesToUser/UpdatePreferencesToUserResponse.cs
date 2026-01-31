using User.API.Features.UserPreference.GetUserPreferences;

namespace User.API.Features.UserPreference.UpdatePreferencesToUser
{
    public record UpdatePreferencesToUserResponse(int Id, string Name, string Email, 
        DateTime CreatedAt, List<PreferencesResponse> Preferences); 
}
