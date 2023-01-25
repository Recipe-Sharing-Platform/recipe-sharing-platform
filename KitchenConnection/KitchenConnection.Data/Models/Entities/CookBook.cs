using KitchenConnection.DataLayer.Models.Entities.Mappings;
using KitchenConnection.Models.Entities;

namespace KitchenConnection.DataLayer.Models.Entities;
public class CookBook : BaseEntity {
    public string Name { get; set; }
    public string Description { get; set; }
    public int NumberOfRecipes { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }

    public List<CookBookRecipe> Recipes { get; set; }
}