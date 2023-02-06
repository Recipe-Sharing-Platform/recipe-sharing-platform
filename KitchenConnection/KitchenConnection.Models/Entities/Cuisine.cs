namespace KitchenConnection.Models.Entities;

public class Cuisine : BaseEntity
{
    public string Name { get; set; }

    public List<Recipe> Recipes { get; set; }
}
