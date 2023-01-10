
using KitchenConnection.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenConnection.DataLayer.Models.Entities; 
public class Review : BaseEntity {
    public string ReviewContent { get; set; }
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public string RecipeId { get; set; }
    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }
}
