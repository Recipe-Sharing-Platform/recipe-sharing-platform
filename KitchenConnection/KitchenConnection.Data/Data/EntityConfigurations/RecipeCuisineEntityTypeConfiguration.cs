using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KitchenConnection.DataLayer.Data.EntityConfigurations {
    public class RecipeCuisineEntityTypeConfiguration : IEntityTypeConfiguration<Cuisine> {
        public void Configure(EntityTypeBuilder<Cuisine> builder) {
            builder.HasKey(k => k.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.HasMany(r => r.Recipes).WithOne(c => c.Cuisine);
        }
    }
}
