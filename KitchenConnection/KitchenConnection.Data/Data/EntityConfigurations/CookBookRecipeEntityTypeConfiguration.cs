using KitchenConnection.DataLayer.Models.Entities.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KitchenConnecition.Infrastructure.Data.Configurations
{
    public class CookBookRecipeEntityTypeConfiguration : IEntityTypeConfiguration<CookBookRecipe>
    {
        public void Configure(EntityTypeBuilder<CookBookRecipe> builder)
        {
            //builder.HasKey(k => new {k.CookBookId, k.RecipeId});
            builder.HasOne(c => c.CookBook).WithMany(r => r.Recipes).HasForeignKey(x => x.CookBookId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
