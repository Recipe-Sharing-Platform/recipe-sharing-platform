using KitchenConnection.DataLayer.Models.DTOs.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Services.IServices
{
    public interface IShoppingListService
    {
        Task<ShoppingListCreateDTO> AddShoppingList(Guid userId, ShoppingListCreateDTO shoppingList);

    }
}
