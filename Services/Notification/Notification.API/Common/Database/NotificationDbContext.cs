using Microsoft.EntityFrameworkCore;

namespace Notification.API.Common.Database
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.Notification> Notifications { get; set; }
        public DbSet<Entities.NotificationPreference> NotificationPreferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly); 
            base.OnModelCreating(modelBuilder);
        }

        //        GET    /api/notifications              - Notificaciones del usuario(Auth)
        //        PUT    /api/notifications/{id}/read    - Marcar como leída(Auth)
        //        PUT    /api/notifications/read-all     - Marcar todas como leídas(Auth)
        //        POST   /api/notifications/rating-received - (Interno) Rating recibido
        //        GET    /api/notifications/preferences  - Preferencias(Auth)
        //        PUT    /api/notifications/preferences  - Actualizar preferencias(Auth)
    }
}
