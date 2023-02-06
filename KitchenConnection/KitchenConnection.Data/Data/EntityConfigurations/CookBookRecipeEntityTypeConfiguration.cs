using KitchenConnection.Models.Entities;
using KitchenConnection.Models.Entities.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KitchenConnection.DataLayer.Data.EntityConfigurations {
    //public class CookBookRecipeEntityTypeConfiguration : IEntityTypeConfiguration<CookBook>
    //{
    //    public void Configure(EntityTypeBuilder<CookBook> builder)
    //    {
    //        builder.HasKey(k => k.Id);
    //        builder.Property(i => i.Id).ValueGeneratedOnAdd();
    //        builder.HasMany(r => r.Recipes).WithOne(c => c.CookBook).OnDelete(DeleteBehavior.NoAction);
    //    }
    //}
}
