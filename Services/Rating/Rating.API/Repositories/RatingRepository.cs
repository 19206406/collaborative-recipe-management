using Microsoft.EntityFrameworkCore;
using Rating.API.Common.Database;
using Rating.API.Entities;

namespace Rating.API.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly RatingDbContext _context;

        public RatingRepository(RatingDbContext context)
        {
            _context = context;
        }

        public async Task<RatingE> AddRating(RatingE rating)
        {
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return rating; 
        }

        public async Task DeleteRatingAsync(RatingE rating)
        {
            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync(); 
        }

        public async Task<RatingE?> GetRating(int id)
        {
            var rating = await _context.Ratings
                .AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);

            return rating; 
        }

        public Task<List<RatingE>> GetRatingsByRecipeIdAsync(int recipeId)
        {
            var ratings = _context.Ratings
                .AsNoTracking()
                .Where(x => x.RecipeId == recipeId)
                .ToListAsync();

            return ratings; 
        }

        public Task<List<RatingE>> GetRatingsByUserIdAsync(int userId)
        {
            var ratings = _context.Ratings
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return ratings;
        }

        public async Task<RatingE> UpdateRating(RatingE rating)
        {
            _context.Ratings.Update(rating);
            await _context.SaveChangesAsync();

            await _context.Entry(rating).ReloadAsync();
            return rating; 
        }
    }
}
