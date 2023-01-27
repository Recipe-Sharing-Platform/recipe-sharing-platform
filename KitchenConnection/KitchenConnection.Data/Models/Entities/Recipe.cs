using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenConnection.Models.Entities;
public class Recipe : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<RecipeIngredient> Ingredients { get; set; }
    public List<RecipeInstruction> Instructions { get; set; }
    public List<RecipeTag> Tags { get; set; }
    public Guid CuisineId { get; set; }
    public Cuisine Cuisine { get; set; }
    public DateTime PrepTime { get; set; }
    public DateTime CookTime { get; set; }
    public DateTime TotalTime { get; set; }
    public int Servings { get; set; }
    public int Yield { get; set; }
    public double Calories { get; set; }
    public string AudioInstructions { get; set; } // Audio Url
    public string VideoInstructions { get; set; } // Video Url
    public List<Review> Reviews { get; set; } = new List<Review>();
}