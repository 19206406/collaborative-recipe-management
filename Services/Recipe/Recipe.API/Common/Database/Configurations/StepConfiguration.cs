using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipe.API.Entities;

namespace Recipe.API.Common.Database.Configurations
{
    public class StepConfiguration : IEntityTypeConfiguration<Step>
    {
        public void Configure(EntityTypeBuilder<Step> builder)
        {
            builder.HasKey(s => s.Id);

            builder.ToTable("steps");

            builder.Property(s => s.RecipeId)
                .IsRequired()
                .HasColumnName("recipe_id");

            builder.Property(s => s.StepNumber)
                .IsRequired()
                .HasColumnName("step_number");

            builder.Property(s => s.Instruction)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("instruction"); 
        }
    }
}
