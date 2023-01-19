using KitchenConnection.DataLayer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KitchenConnecition.Infrastructure.Data.Configurations
{
    public class RecipeCuisineEntityTypeConfiguration : IEntityTypeConfiguration<Cuisine>
    {
        public void Configure(EntityTypeBuilder<Cuisine> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.HasMany(r => r.Recipes).WithOne(c => c.Cuisine);
        }
    }
}
