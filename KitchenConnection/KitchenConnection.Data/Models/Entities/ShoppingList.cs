using KitchenConnection.Models.Entities;

namespace KitchenConnection.DataLayer.Models.Entities
{
    public class ShoppingList:BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public List<ShoppingListItem> ShoppingListItems { get; set; }
    }
}