using KitchenConnection.DataLayer.Models.Entities.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KitchenConnecition.Infrastructure.Data.Configurations
{
    public class CollectionRecipeEntityTypeConfiguration : IEntityTypeConfiguration<CollectionRecipe>
    {
        public void Configure(EntityTypeBuilder<CollectionRecipe> builder)
        {
            //builder.HasKey(k => new {k.CollectionId, k.RecipeId});
            builder.HasOne(r => r.Collection).WithMany(r => r.Recipes).HasForeignKey(r => r.CollectionId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
