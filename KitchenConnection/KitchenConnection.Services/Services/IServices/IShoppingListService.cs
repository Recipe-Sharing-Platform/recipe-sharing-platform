using KitchenConnection.Models.DTOs.ShoppingCart;
using KitchenConnection.Models.Entities;

namespace KitchenConnection.BusinessLogic.Services.IServices {
    public interface IShoppingListService
    {
        Task<List<ShoppingListItemCreateDTO>> AddToShoppingList(Guid userId, List<ShoppingListItemCreateDTO> shoppingList);
        Task<bool> DeleteFromShoppingList(Guid userId, Guid shoppingListItemIds);
        Task<ShoppingListItem> GetShoppingListItemById(Guid userId, Guid shoppingListItemId);
        Task<string> GetShoppingListItemUrl(Guid userId, Guid shoppingListItemId);
        Task<List<ShoppingListItem>> GetShoppingListForUser(Guid userId);

    }
}
