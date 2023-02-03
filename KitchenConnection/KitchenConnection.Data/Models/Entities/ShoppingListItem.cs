using KitchenConnection.DataLayer.Models.Enums;
using KitchenConnection.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.DataLayer.Models.Entities
{
    public class ShoppingListItem:BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
