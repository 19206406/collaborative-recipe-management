using User.API.Features.UserPreference.GetUserPreferences;

namespace User.API.Features.UserPreference.AddPreferencesToUser
{
    public record AddPreferencesToUserResponse(int Id, string Name, string Email, DateTime CreatedAt, List<PreferencesResponse> Preferences); 
}
