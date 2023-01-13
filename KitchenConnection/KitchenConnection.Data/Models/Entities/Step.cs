using KitchenConnection.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenConnection.DataLayer.Models.Entities; 
public class Step : BaseEntity {
    public string RecipeId { get; set; }
    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }
    public int StepNumber { get; set; }
    public string StepDescription { get; set; }
}
