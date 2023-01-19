using KitchenConnection.Models.Entities;

namespace KitchenConnection.DataLayer.Models.Entities;

public class Cuisine : BaseEntity
{
    public string Name { get; set; }

    public List<Recipe> Recipes { get; set; }
}
