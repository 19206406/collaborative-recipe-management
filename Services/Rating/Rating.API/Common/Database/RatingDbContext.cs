using Microsoft.EntityFrameworkCore;
using Rating.API.Entities;

namespace Rating.API.Common.Database
{
    public class RatingDbContext : DbContext
    {
        public RatingDbContext(DbContextOptions<RatingDbContext> options) : base(options)
        {
        }

        public DbSet<RatingE> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RatingDbContext).Assembly); 
            base.OnModelCreating(modelBuilder);
        }


        // DELETE /api/ratings/{id}  - Eliminar calificación(Auth) -- listo 
        // GET    /api/ratings/recipe/{recipeId}/average - Promedio -- listo 
        // GET    /api/ratings/user/{userId}/recipe/{recipeId} - Rating específico -- listo 
    }
}
