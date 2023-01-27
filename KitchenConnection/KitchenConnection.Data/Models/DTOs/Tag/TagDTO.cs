using KitchenConnection.DataLayer.Models.DTOs.RecipeTag;
using KitchenConnection.DataLayer.Models.Entities.Mappings;

namespace KitchenConnection.Application.Models.DTOs.Recipe
{
    public class TagDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<RecipeTagDTO> Recipes { get; set; } = new List<RecipeTagDTO>();
    }
}
