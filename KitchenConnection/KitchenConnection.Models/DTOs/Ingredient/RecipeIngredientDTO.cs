using KitchenConnection.Models.Enums;

namespace KitchenConnection.Models.DTOs.Ingredient
{
    public class RecipeIngredientDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public Unit Unit { get; set; }
    }
}
