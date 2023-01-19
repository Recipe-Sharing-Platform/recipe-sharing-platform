using KitchenConnection.DataLayer.Models.Entities.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KitchenConnecition.Infrastructure.Data.Configurations
{
    public class RecipeIngredientEntityTypeConfiguration : IEntityTypeConfiguration<RecipeIngredient>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {
            builder.HasOne(r => r.Recipe).WithMany(i => i.Ingredients).HasForeignKey(r => r.RecipeId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
