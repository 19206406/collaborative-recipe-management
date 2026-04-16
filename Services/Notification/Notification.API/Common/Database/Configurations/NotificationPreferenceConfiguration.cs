using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.API.Entities;

namespace Notification.API.Common.Database.Configurations
{
    public class NotificationPreferenceConfiguration : IEntityTypeConfiguration<Entities.NotificationPreference>
    {
        public void Configure(EntityTypeBuilder<NotificationPreference> builder)
        {
            builder.ToTable("notification_preferences");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.HasIndex(x => x.UserId)
                .IsUnique(); 

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasColumnName("user_id");

            builder.Property(x => x.EmailNotifications)
                .IsRequired()
                .HasColumnName("email_notifications");

            builder.Property(x => x.PushNotifications)
                .IsRequired()
                .HasColumnName("push_notifications"); 
        }
    }
}
