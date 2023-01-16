using KitchenConnection.DataLayer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenConnection.Models.Entities; 
public class Recipe : BaseEntity {
    public string Name { get; set; }
    public string Description { get; set; }
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public User? User { get; set; }
    [NotMapped]
    public List<Step> Steps { get; set; }
}