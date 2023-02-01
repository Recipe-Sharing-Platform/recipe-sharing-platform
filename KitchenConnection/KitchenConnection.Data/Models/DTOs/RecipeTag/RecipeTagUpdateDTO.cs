using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.DataLayer.Models.DTOs.RecipeTag
{
    public class RecipeTagUpdateDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
