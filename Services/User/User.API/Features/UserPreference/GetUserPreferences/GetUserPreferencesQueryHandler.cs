using BuildingBlocks.CQRS;
using User.API.repositories.UserPreferenceRepository;

namespace User.API.Features.UserPreference.GetUserPreferences
{
    public class GetUserPreferencesQueryHandler : IQueryHandler<GetUserPreferencesQuery, GetUserPreferencesResponse>
    {
        private readonly IUserPreferenceRespository _userPreferenceRespository;

        public GetUserPreferencesQueryHandler(IUserPreferenceRespository userPreferenceRespository)
        {
            _userPreferenceRespository = userPreferenceRespository;
        }

        public async Task<GetUserPreferencesResponse> Handle(GetUserPreferencesQuery query, CancellationToken cancellationToken)
        {
            var user = await _userPreferenceRespository.GetUserPreferences(query.Id);

            if (user is null)
                throw new Exception();

            var preferences = user.UserPreferences.Select(x => new PreferencesResponse(x.Id, x.PreferenceType)).ToList();

            var response = new GetUserPreferencesResponse(user.Id, user.Name, user.Email, user.CreatedAt, user.IsActive, preferences);

            return response; 
        }
    }
}
