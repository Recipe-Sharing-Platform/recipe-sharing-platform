using KitchenConnection.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenConnection.DataLayer.Models.Entities; 
public class RecipeTag : BaseEntity {
    public string RecipeId { get; set; }
    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }
    public string TagId { get; set; }
    [ForeignKey("TagId")]
    public Tag Tag { get; set; }
}
