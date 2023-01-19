using KitchenConnection.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.DataLayer.Models.Entities.Mappings
{
    public class CookBookRecipe : BaseEntity
    {
        public Guid CookBookId { get; set; }
        public CookBook CookBook { get; set; }
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
