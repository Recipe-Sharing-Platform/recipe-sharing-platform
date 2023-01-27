using KitchenConnection.DataLayer.Models.DTOs;
using KitchenConnection.DataLayer.Models.Entities;

namespace KitchenConnection.Application.Models.DTOs.Recipe
{
    public class RecipeDTO
    {
        public Guid Id { get; set; }
        public UserDTO User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<TagDTO> Tags { get; set; }
        public CuisineDTO Cuisine { get; set; }
        public List<RecipeIngredientDTO> Ingredients { get; set; }
        public DateTime PrepTime { get; set; }
        public DateTime CookTime { get; set; }
        public DateTime TotalTime { get; set; }
        public List<RecipeInstructionDTO> Instructions { get; set; }
        public int Servings { get; set; }
        public int Yield { get; set; }
        public double Calories { get; set; }
        public string AudioInstructions { get; set; } // Audio Url
        public string VideoInstructions { get; set; } // Video Url
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}
