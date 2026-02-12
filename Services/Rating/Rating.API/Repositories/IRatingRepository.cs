using Rating.API.Entities;

namespace Rating.API.Repositories
{
    public interface IRatingRepository
    {
        Task<RatingE> AddRating(RatingE rating);

        Task<RatingE> UpdateRating(RatingE rating);

        Task<RatingE?> GetRating(int id);

        Task<List<RatingE>> GetRatingsByRecipeIdAsync(int recipeId);

        Task<List<RatingE>> GetRatingsByUserIdAsync(int userId);

        Task DeleteRatingAsync(RatingE rating);

        Task<double> GetAverageRatingAsync(int recipeId);

        Task<int> GetSpecificRatingAsync(int userId, int recipeId); 
    }
}
