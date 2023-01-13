using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KitchenConnection.DataLayer.Data;

public class KitchenConnectionDbContext : DbContext {

	public KitchenConnectionDbContext(DbContextOptions<KitchenConnectionDbContext> options) : base(options) { }

	public DbSet<Recipe> Recipes { get; set; }
	public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
	public DbSet<Step> Steps { get; set; }
	public DbSet<Tag> Tags { get; set; }
	public DbSet<RecipeTag> RecipeTags { get; set; }
	public DbSet<CookBook> CookBooks { get; set; }
	public DbSet<RecipeCookBook> RecipeCookBooks { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.Entity<RecipeCookBook>()
			.HasOne(r => r.Recipe)
			.WithMany()
			.HasForeignKey(rc => rc.RecipeId)
			.OnDelete(DeleteBehavior.Restrict);
		modelBuilder.Entity<Review>()
			.HasOne(r => r.User)
			.WithMany()
			.HasForeignKey(r => r.UserId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}