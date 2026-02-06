using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Recipe.API.Common.Database.Configurations
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Entities.Recipe>
    {
        public void Configure(EntityTypeBuilder<Entities.Recipe> builder)
        {
            builder.ToTable("recipes");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("id"); 

            builder.Property(r => r.UserId)
                .IsRequired()
                .HasColumnName("user_id");

            builder.HasIndex(r => r.UserId); 

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
                .HasMaxLength(40)
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
                .HasColumnName("created_at");

            builder.Property(r => r.UpdatedAt)
                .IsRequired()
                .HasColumnName("updated_at");

            // relaciones 

            builder.HasMany(r => r.Ingredients)
                .WithOne(i => i.Recipe)
                .HasForeignKey(r => r.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.Steps)
                .WithOne(s => s.Recipe)
                .HasForeignKey(r => r.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.RecipeTags)
                .WithOne(rt => rt.Recipe)
                .HasForeignKey(rt => rt.RecipeId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
