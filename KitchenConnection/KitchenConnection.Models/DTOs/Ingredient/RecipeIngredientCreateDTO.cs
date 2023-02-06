using KitchenConnection.Models.Enums;

namespace KitchenConnection.Models.DTOs.Ingredient
{
    public class RecipeIngredientCreateDTO
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public Unit Unit { get; set; }
    }
}
