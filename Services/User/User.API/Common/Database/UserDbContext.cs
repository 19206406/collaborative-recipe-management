using Microsoft.EntityFrameworkCore;
using User.API.Entities;

namespace User.API.Common.Database
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        // entities 
        public DbSet<Entities.User> Users { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly); 
            base.OnModelCreating(modelBuilder);
        }

        //GET    /api/users/profile           - Perfil propio(Auth)
        //PUT    /api/users/profile           - Actualizar perfil(Auth)
        //GET    /api/users/{id}/preferences  - Preferencias de usuario
        //PUT    /api/users/preferences       - Actualizar preferencias(Auth)
        //GET    /api/users/{id}/ basic - Info básica(para otros servicios)
    }
}
