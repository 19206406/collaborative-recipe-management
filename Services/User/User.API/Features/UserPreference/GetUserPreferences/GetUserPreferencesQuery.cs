using BuildingBlocks.CQRS;

namespace User.API.Features.UserPreference.GetUserPreferences
{
    public record GetUserPreferencesQuery(int Id) : IQuery<GetUserPreferencesResponse>; 
}
