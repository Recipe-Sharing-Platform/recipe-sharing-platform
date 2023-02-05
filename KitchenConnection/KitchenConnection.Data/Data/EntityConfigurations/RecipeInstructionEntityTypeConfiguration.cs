using KitchenConnection.Models.Entities.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KitchenConnection.DataLayer.Data.EntityConfigurations {
    public class RecipeInstructionEntityTypeConfiguration : IEntityTypeConfiguration<RecipeInstruction> {
        public void Configure(EntityTypeBuilder<RecipeInstruction> builder) {
            builder.HasOne(r => r.Recipe).WithMany(i => i.Instructions).HasForeignKey(fk => fk.RecipeId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
