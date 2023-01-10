using Microsoft.EntityFrameworkCore;

namespace KitchenConnection.DataLayer.Data;

public class KitchenConnectionDbContext : DbContext {

	public KitchenConnectionDbContext(DbContextOptions<KitchenConnectionDbContext> options) : base(options) { }
}