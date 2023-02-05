namespace KitchenConnection.Models.Entities;
public class CookBook : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public List<Recipe> Recipes { get; set; }
}