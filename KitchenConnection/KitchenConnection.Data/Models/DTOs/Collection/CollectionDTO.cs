using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.DataLayer.Models.DTOs.Collection
{
    public class CollectionDTO
    {
        public Guid Id { get; set; }
        public UserDTO User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfRecipes { get; set; }

        public List<RecipeDTO>? Recipes { get; set; }
    }
}
