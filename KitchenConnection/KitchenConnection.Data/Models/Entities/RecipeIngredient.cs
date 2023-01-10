using KitchenConnection.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenConnection.DataLayer.Models.Entities; 
public class RecipeIngredient : BaseEntity {
    public string IngredientName { get; set; }
    public string RecipeId { get; set; }
    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }
    public string IngredientId { get; set; }
    public AmountType AmountType { get; set; }
    public int Amount { get; set; }
}

public enum AmountType {
    TableSpoon,
    TeaSpoon,
    Liter,
    Gram,
    // add more
}