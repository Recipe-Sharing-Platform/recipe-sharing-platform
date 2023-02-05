using KitchenConnection.Models.Entities.Mappings;

namespace KitchenConnection.Models.Entities;
public class Recipe : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<RecipeIngredient> Ingredients { get; set; }
    public List<RecipeInstruction> Instructions { get; set; }
    public List<Tag>? Tags { get; set; }
    public Guid CuisineId { get; set; }
    public Cuisine Cuisine { get; set; }
    public int PrepTime { get; set; }
    public int CookTime { get; set; }
    public int Servings { get; set; }
    public int Yield { get; set; }
    public double Calories { get; set; }
    public string AudioInstructions { get; set; } // Audio Url
    public string VideoInstructions { get; set; } // Video Url

    public List<Review>? Reviews { get; set; }
    public CookBook? CookBook { get; set; } // The CookBook that this recipe is in or not

}