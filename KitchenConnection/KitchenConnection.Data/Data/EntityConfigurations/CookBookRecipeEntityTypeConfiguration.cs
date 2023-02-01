using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KitchenConnecition.Infrastructure.Data.Configurations
{
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
