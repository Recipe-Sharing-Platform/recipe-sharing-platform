using System.ComponentModel.DataAnnotations.Schema;
using KitchenConnection.Models.Entities;

namespace KitchenConnection.DataLayer.Models.Entities;

public class RecipeCookBook : BaseEntity {
    public string RecipeId { get; set; }
    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }
    public string CookBookId { get; set; }
    [ForeignKey("CookBookId")]
    public CookBook CookBook { get; set; }
}
