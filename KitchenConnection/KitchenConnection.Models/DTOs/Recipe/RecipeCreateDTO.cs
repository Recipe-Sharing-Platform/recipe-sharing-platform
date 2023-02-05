using KitchenConnection.Models.DTOs.Ingredient;
using KitchenConnection.Models.DTOs.Instruction;
using KitchenConnection.Models.DTOs.Tag;

namespace KitchenConnection.Models.DTOs.Recipe
{
    public class RecipeCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CuisineId { get; set; }
        public List<TagCreateDTO> Tags { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public List<RecipeIngredientCreateDTO> Ingredients { get; set; }
        public List<RecipeInstructionCreateDTO> Instructions { get; set; }
        public int Servings { get; set; }
        public int Yield { get; set; }
        public double Calories { get; set; }
        public string AudioInstructions { get; set; } // Audio Url
        public string VideoInstructions { get; set; } // Video Url

    }
}