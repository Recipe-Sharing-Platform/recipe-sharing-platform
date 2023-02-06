using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.Models.DTOs.Cuisine
{
    public class CuisineUpdateDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
