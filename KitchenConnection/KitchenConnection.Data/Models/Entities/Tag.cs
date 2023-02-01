
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using KitchenConnection.Models.Entities;

namespace KitchenConnection.DataLayer.Models.Entities;
public class Tag : BaseEntity
{
    public string Name { get; set; }

    public List<Recipe> Recipes { get; set; }
}
