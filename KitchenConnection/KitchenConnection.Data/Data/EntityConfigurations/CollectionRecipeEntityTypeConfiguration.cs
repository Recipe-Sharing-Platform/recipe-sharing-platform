using KitchenConnection.Models.Entities.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KitchenConnection.DataLayer.Data.EntityConfigurations {
    public class CollectionRecipeEntityTypeConfiguration : IEntityTypeConfiguration<CollectionRecipe> {
        public void Configure(EntityTypeBuilder<CollectionRecipe> builder) {
            //builder.HasKey(k => new {k.CollectionId, k.RecipeId});
            builder.HasOne(r => r.Collection).WithMany(r => r.Recipes).HasForeignKey(r => r.CollectionId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
