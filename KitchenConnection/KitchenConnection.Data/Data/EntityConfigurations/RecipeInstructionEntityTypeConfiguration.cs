using KitchenConnection.DataLayer.Models.Entities.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KitchenConnecition.Infrastructure.Data.Configurations
{
    public class RecipeInstructionEntityTypeConfiguration : IEntityTypeConfiguration<RecipeInstruction>
    {
        public void Configure(EntityTypeBuilder<RecipeInstruction> builder)
        {
            builder.HasOne(r => r.Recipe).WithMany(i => i.Instructions).HasForeignKey(fk => fk.RecipeId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
