namespace User.API.Features.UserPreference.GetUserPreferences
{
    public record PreferencesResponse(int Id, string PreferenceType);
    public record GetUserPreferencesResponse(
        int Id, string Name, string Email, DateTime CreatedAt, List<PreferencesResponse> Preferences); 
}
