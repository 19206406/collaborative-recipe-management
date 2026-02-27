using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.API.Entities;

namespace User.API.Common.Database.Configurations
{
    public class UserPreferenceConfiguration : IEntityTypeConfiguration<UserPreference>
    {
        public void Configure(EntityTypeBuilder<UserPreference> builder)
        {
            builder.ToTable("user_preferences");

            builder.HasKey(up => up.Id);

            builder.Property(up => up.Id)
                .HasColumnName("id");

            builder.Property(up => up.UserId)
                .HasColumnName("user_id"); 

            builder.Property(up => up.PreferenceType)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("reference_type");

            builder.Property(up => up.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at"); 
        }
    }
}
