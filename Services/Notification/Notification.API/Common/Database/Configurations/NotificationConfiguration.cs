using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notification.API.Common.Database.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Entities.Notification>
    {
        public void Configure(EntityTypeBuilder<Entities.Notification> builder)
        {
            builder.ToTable("notifications");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasColumnName("user_id");

            builder.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("type");

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("title");

            builder.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("message");

            builder.Property(x => x.RelatedEntityId)
                .HasColumnName("related_entity_id");

            builder.Property(x => x.IsRead)
                .IsRequired()
                .HasColumnName("is_read");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at"); 
        }
    }
}
