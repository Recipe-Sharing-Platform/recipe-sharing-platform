using KitchenConnection.DataLayer.Models.Enums;

namespace KitchenConnection.Application.Models.DTOs.Recipe
{
    public class RecipeIngredientCreateDTO
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public Unit Unit { get; set; }
    }
}
