using KitchenConnection.DataLayer.Models.DTOs.RecipeTag;
using KitchenConnection.DataLayer.Models.Entities.Mappings;

namespace KitchenConnection.Application.Models.DTOs.Recipe
{
    public class TagCreateDTO
    {
        public string Name { get; set; }
        public List<RecipeTagCreateDTO> Recipes { get; set; } = new List<RecipeTagCreateDTO>();
    }
}