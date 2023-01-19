using KitchenConnection.DataLayer.Models.Entities.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KitchenConnecition.Infrastructure.Data.Configurations
{
    public class RecipeTagEntityTypeConfiguration : IEntityTypeConfiguration<RecipeTag>
    {
        public void Configure(EntityTypeBuilder<RecipeTag> builder)
        {
            builder.HasKey(i => new { i.RecipeId, i.TagId });
            builder.HasOne(t => t.Tag).WithMany(r => r.Recipes).HasForeignKey(t => t.TagId);
            builder.HasOne(r => r.Recipe).WithMany(t => t.Tags).HasForeignKey(t => t.RecipeId);
        }
    }
}