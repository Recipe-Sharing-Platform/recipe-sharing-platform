using KitchenConnection.DataLayer.Models.Enums;

namespace KitchenConnection.DataLayer.Models.DTOs.Recipe
{
    public class RecipeIngredientDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public Unit Unit { get; set; }
    }
}
