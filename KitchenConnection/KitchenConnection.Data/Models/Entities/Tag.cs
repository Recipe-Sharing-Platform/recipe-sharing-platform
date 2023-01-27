
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using KitchenConnection.Models.Entities;

namespace KitchenConnection.DataLayer.Models.Entities;
public class Tag : BaseEntity
{
    public string Name { get; private set; }

    public List<RecipeTag> Recipes { get; set; }=new List<RecipeTag>();
}
