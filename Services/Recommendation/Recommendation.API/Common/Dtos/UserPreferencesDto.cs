namespace Recommendation.API.Common.Dtos
{
    public record PreferencesResponse(int Id, string PreferenceType);

    public record UserPreferencesDto(
        int Id, string Name, string Email, DateTime CreatedAt, List<PreferencesResponse> Preferences);

}
