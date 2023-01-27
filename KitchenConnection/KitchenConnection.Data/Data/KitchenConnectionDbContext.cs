using KitchenConnecition.Infrastructure.Data.Configurations;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KitchenConnection.DataLayer.Data;

public class KitchenConnectionDbContext : DbContext {

	public KitchenConnectionDbContext(DbContextOptions<KitchenConnectionDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<RecipeTag> RecipeTags { get; set; }
    public DbSet<Cuisine> Cuisines { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<RecipeInstruction> InstructionSteps { get; set; }
    public DbSet<CookBook> CookBook { get; set; }
    public DbSet<CookBookRecipe> CookBookRecipes { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<CollectionRecipe> CollectionRecipes { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new RecipeIngredientEntityTypeConfiguration().Configure(modelBuilder.Entity<RecipeIngredient>());
        new RecipeInstructionEntityTypeConfiguration().Configure(modelBuilder.Entity<RecipeInstruction>());

        new CookBookRecipeEntityTypeConfiguration().Configure(modelBuilder.Entity<CookBookRecipe>());
        new CollectionRecipeEntityTypeConfiguration().Configure(modelBuilder.Entity<CollectionRecipe>());

        new RecipeTagEntityTypeConfiguration().Configure(modelBuilder.Entity<RecipeTag>());
        new RecipeCuisineEntityTypeConfiguration().Configure(modelBuilder.Entity<Cuisine>());

        modelBuilder.Entity<Recipe>().Property(r => r.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Recipe>().HasOne(u => u.User).WithMany(r => r.Recipes)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Recipe>().HasMany(r => r.Reviews).WithOne(r => r.Recipe).HasForeignKey(r => r.RecipeId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Recipe>().Property(r => r.Tags).HasDefaultValue(new List<RecipeTag>());//if no tags on recipe creation are added
        
        modelBuilder.Entity<Tag>().Property(t => t.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<User>().HasMany(r => r.Recipes).WithOne(u => u.User)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}