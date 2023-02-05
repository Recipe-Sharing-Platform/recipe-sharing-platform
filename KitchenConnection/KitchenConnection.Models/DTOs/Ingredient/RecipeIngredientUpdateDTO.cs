using KitchenConnection.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.Models.DTOs.Ingredient
{
    public class RecipeIngredientUpdateDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public Unit Unit { get; set; }
    }
}
