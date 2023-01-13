
using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenConnection.DataLayer.Models.Entities; 
public class CookBook : BaseEntity {
    public string Name { get; set; }
    public int RecipeNumber { get; set; }
    public string Description { get; set; }
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    [NotMapped]
    public List<Recipe> Recipes { get; set; }
}