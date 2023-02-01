using KitchenConnection.DataLayer.Models.Entities.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KitchenConnecition.Infrastructure.Data.Configurations;

//public class RecipeTagEntityTypeConfiguration : IEntityTypeConfiguration<RecipeTag>
//{
//    public void Configure(EntityTypeBuilder<RecipeTag> builder)
//    {
//        builder.HasKey(k => new { k.RecipeId, k.TagId });
//        builder.HasOne(t => t.Tag).WithMany(r => r.Recipes).HasForeignKey(fk => fk.TagId);
//        builder.HasOne(r => r.Recipe).WithMany(t => t.Tags).HasForeignKey(fk => fk.RecipeId).OnDelete(DeleteBehavior.Cascade);
//    }
//}