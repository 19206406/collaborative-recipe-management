using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipe.API.Entities;

namespace Recipe.API.Common.Database.Configurations
{
    public class RecipeTagConfiguration : IEntityTypeConfiguration<RecipeTag>
    {
        public void Configure(EntityTypeBuilder<RecipeTag> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.ToTable("recipe_tags");

            // foreign key 

            builder.Property(rt => rt.Tag)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("tag");

            builder.HasIndex(rt => new { rt.RecipeId, rt.Tag })
                .IsUnique()
                .HasDatabaseName("IX_Recipe_Tag"); 
        }
    }
}
