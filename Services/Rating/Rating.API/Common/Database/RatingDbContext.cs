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
    }
}
