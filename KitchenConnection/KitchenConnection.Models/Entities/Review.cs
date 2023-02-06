using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenConnection.Models.Entities;
public class Review : BaseEntity
{
    public Guid UserId { get; set; } // User who made the review
    public User User { get; set; }
    public Guid RecipeId { get; set; } // Recipe the review corresponds to
    public Recipe Recipe { get; set; }
    public double Rating { get; set; }
    public string? Message { get; set; } // Review Message
}
