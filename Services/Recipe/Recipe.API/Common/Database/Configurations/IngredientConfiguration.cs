using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipe.API.Entities;

namespace Recipe.API.Common.Database.Configurations
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("ingredients");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .IsRequired()
                .HasColumnName("id");

            builder.Property(i => i.RecipeId)
                .IsRequired()
                .HasColumnName("recipe_id"); 

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("title");

            builder.Property(i => i.Quantity)
                .IsRequired()
                .HasColumnType("decimal(10, 2)")
                //.HasPrecision(10, 2)
                .HasColumnName("quantity");

            builder.Property(i => i.Unit)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("unit");

            builder.Property(i => i.DisplayOrder)
                .IsRequired()
                .HasColumnName("display_order"); 
        }
    }
}
