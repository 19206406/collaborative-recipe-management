using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Recipe.API.Common.Database.Configurations
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Entities.Recipe>
    {
        public void Configure(EntityTypeBuilder<Entities.Recipe> builder)
        {

            builder.HasKey(r => r.Id);

            builder.ToTable("recipes");

            // foreign key usuario 

            builder.Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("title");

            builder.Property(r => r.Description)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("description");

            builder.Property(r => r.PrepTimeMinutes)
                .IsRequired()
                .HasColumnName("prep_time_minutes");

            builder.Property(r => r.CookTimeMinutes)
                .IsRequired()
                .HasColumnName("cook_time_minutes");

            builder.Property(r => r.Difficulty)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("difficulty");

            builder.Property(r => r.Servings)
                .IsRequired()
                .HasColumnName("servings");

            builder.Property(r => r.ImageUrl)
                .HasColumnType("text")
                .HasColumnName("image_url");

            builder.Property(r => r.AverageRating)
                .HasColumnType("decimal(3, 2)")
                .HasDefaultValue(0.00)
                .HasColumnName("average_rating");

            builder.Property(r => r.RatingCount)
                .HasDefaultValue(0)
                .HasColumnName("rating_count");

            builder.Property(r => r.CreatedAt)
                .IsRequired()
                .HasDefaultValue(DateTime.UtcNow)
                .HasColumnName("created_at");

            builder.Property(r => r.UpdatedAt)
                .IsRequired()
                .HasDefaultValue(DateTime.UtcNow)
                .HasColumnName("updated_at"); 
        }
    }
}
