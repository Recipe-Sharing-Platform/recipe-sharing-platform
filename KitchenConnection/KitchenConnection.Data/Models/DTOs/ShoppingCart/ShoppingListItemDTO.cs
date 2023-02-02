using KitchenConnection.DataLayer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.DataLayer.Models.DTOs.ShoppingCart
{
    public class ShoppingListItemDTO
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Unit Unit { get; set; }
    }
}
