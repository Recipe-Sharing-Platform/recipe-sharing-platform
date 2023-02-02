using KitchenConnection.DataLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.DataLayer.Models.DTOs.ShoppingCart
{
    public class ShoppingListCreateDTO
    {
        public Guid UserId { get; set; }
        public List<ShoppingListItemDTO> ShoppingListItems { get; set; }

    }
}
