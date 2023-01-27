namespace KitchenConnection.Elastic.Models; 
public class DeleteRecipe : IMessage {
    public Guid RecipeId { get; set; }
}
