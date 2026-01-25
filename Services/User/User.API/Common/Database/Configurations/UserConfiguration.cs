using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.API.Common.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<Entities.User>
    {
        public void Configure(EntityTypeBuilder<Entities.User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name");

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("email");

            builder.HasIndex(u => u.Email)
                .IsUnique(); 

            builder.Property(u => u.PasswordHash)
                //.IsRequired() por el momento todavía no 
                .HasColumnType("nvarchar(max)")
                .HasColumnName("password_hash");

            builder.Property(u => u.CreatedAt) // en el handler 
                .IsRequired()
                .HasColumnName("created_at");

            builder.Property(u => u.UpdatedAt) // en el handler 
                .IsRequired()
                .HasColumnName("updated_at");

            builder.Property(u => u.IsActive) // en el handler 
                .HasColumnName("is_active"); 

            builder.HasMany(u => u.UserPreferences)
                .WithOne(up => up.User)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}