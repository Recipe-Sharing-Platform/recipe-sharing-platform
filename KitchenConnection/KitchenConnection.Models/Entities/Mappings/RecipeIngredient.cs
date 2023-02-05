using KitchenConnection.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.Models.Entities.Mappings
{
    public class RecipeIngredient : BaseEntity
    {
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; } // Recipe that the ingredient corresponds to
        public string Name { get; set; }
        public double Amount { get; set; }
        public Unit Unit { get; set; }
    }
}
