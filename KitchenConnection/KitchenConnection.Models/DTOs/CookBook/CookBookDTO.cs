using KitchenConnection.Models.DTOs.Recipe;

namespace KitchenConnection.Models.DTOs.CookBook;

public class CookBookDTO
{
    public Guid Id { get; set; }
    public UserDTO User { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int NumberOfRecipes { get; set; }

    public List<RecipeDTO>? Recipes { get; set; }
}
