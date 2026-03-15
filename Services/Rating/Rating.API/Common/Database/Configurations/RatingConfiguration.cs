using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rating.API.Entities;

namespace Rating.API.Common.Database.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<RatingE>
    {
        public void Configure(EntityTypeBuilder<RatingE> builder)
        {
            builder.ToTable("ratings");

            builder.HasKey(r => r.Id);

            builder.HasIndex(r => new { r.UserId, r.RecipeId })
                .IsUnique();

            builder.Property(r => r.Id)
                .HasColumnName("id");

            builder.Property(r => r.UserId)
                .IsRequired()
                .HasColumnName("user_id");

            builder.Property(r => r.RecipeId)
                .IsRequired()
                .HasColumnName("recipe_id"); 

            builder.Property(r => r.Rating)
                .IsRequired()
                .HasColumnName("rating");

            builder.Property(r => r.Comment)
                .HasMaxLength(500)
                .HasColumnName("comment");

            builder.Property(r => r.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at");

            builder.Property(r => r.UpdatedAt)
                .IsRequired()
                .HasColumnName("updated_at");

        }
    }
}
