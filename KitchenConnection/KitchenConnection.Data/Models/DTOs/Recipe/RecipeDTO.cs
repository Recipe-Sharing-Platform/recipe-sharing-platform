using KitchenConnection.Application.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.DTOs;

namespace KitchenConnection.DataLayer.Models.DTOs.Recipe;

public class RecipeDTO
{
    public Guid Id { get; set; }
    public UserDTO User { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public CuisineDTO Cuisine { get; set; }
    public List<TagDTO> Tags { get; set; }
    public int PrepTime { get; set; }
    public int CookTime { get; set; }
    public int TotalTime { get; set; }
    public List<RecipeIngredientDTO> Ingredients { get; set; }
    public List<RecipeInstructionDTO> Instructions { get; set; }
    public int Servings { get; set; }
    public int Yield { get; set; }
    public double Calories { get; set; }
    public string AudioInstructions { get; set; } // Audio Url
    public string VideoInstructions { get; set; } // Video Url
}
