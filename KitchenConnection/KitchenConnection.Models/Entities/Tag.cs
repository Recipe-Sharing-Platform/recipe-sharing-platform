using KitchenConnection.Models.Entities.Mappings;

namespace KitchenConnection.Models.Entities;
public class Tag : BaseEntity
{
    public string Name { get; set; }

    public List<Recipe> Recipes { get; set; }
}
