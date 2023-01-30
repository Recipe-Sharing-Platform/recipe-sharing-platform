using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.DataLayer.Models.DTOs.Collection
{
    public class CollectionCreateDTO
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Guid> Recipes { get; set; }
    }
}
