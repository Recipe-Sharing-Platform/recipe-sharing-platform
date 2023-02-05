using KitchenConnection.Models.Entities;
using KitchenConnection.Models.Entities.Mappings;
using Microsoft.EntityFrameworkCore;
using KitchenConnection.DataLayer.Data.EntityConfigurations;

namespace KitchenConnection.DataLayer.Data;

public class KitchenConnectionDbContext : DbContext {

	public KitchenConnectionDbContext(DbContextOptions<KitchenConnectionDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Cuisine> Cuisines { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<RecipeInstruction> InstructionSteps { get; set; }
    public DbSet<CookBook> CookBook { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<CollectionRecipe> CollectionRecipes { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<RecommendationScore> RecommendationScore { get; set; }
    public DbSet<ShoppingListItem> ShoppingListItem { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new RecipeIngredientEntityTypeConfiguration().Configure(modelBuilder.Entity<RecipeIngredient>());
        new RecipeInstructionEntityTypeConfiguration().Configure(modelBuilder.Entity<RecipeInstruction>());

        //new CookBookRecipeEntityTypeConfiguration().Configure(modelBuilder.Entity<CookBook>());
        new CollectionRecipeEntityTypeConfiguration().Configure(modelBuilder.Entity<CollectionRecipe>());

        new RecipeCuisineEntityTypeConfiguration().Configure(modelBuilder.Entity<Cuisine>());

        modelBuilder.Entity<Recipe>().Property(r => r.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Recipe>().HasOne(u => u.User).WithMany(r => r.Recipes)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Recipe>().HasMany(r => r.Instructions).WithOne(r => r.Recipe)
            .HasForeignKey(r => r.RecipeId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Recipe>().HasMany(r => r.Ingredients).WithOne(r => r.Recipe)
            .HasForeignKey(r => r.RecipeId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Recipe>().HasMany(r => r.Reviews).WithOne(r => r.Recipe)
            .HasForeignKey(r => r.RecipeId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Recipe>().HasMany(t => t.Tags).WithMany(r => r.Recipes);
        modelBuilder.Entity<Recipe>().HasOne(c => c.CookBook).WithMany(r => r.Recipes);
        modelBuilder.Entity<CookBook>().HasMany(r => r.Recipes).WithOne(c => c.CookBook).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<CollectionRecipe>().HasOne(c=>c.Collection).WithMany(r => r.Recipes);
        modelBuilder.Entity<Collection>().HasMany(r => r.Recipes).WithOne(c => c.Collection).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Tag>().Property(t => t.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Tag>().HasIndex(t => t.Name).IsUnique();

        modelBuilder.Entity<CookBook>().Property(c => c.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<RecommendationScore>().Property(r => r.Id).ValueGeneratedOnAdd();

        // TODO: What to do with the recipes when the user is deleted?
        modelBuilder.Entity<User>().HasMany(r => r.Recipes).WithOne(u => u.User)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Review>().Property(i => i.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<Review>().HasKey(k => k.Id);
        modelBuilder.Entity<Review>().HasOne(u => u.User).WithMany(r => r.Reviews)
            .HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Recipe>().HasMany(r => r.Reviews).WithOne(r => r.Recipe)
            .HasForeignKey(r => r.RecipeId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ShoppingListItem>().HasKey(e => new { e.Id, e.UserId, e.Name });
        modelBuilder.Entity<ShoppingListItem>().Property(r => r.Id).ValueGeneratedOnAdd();
    }
}