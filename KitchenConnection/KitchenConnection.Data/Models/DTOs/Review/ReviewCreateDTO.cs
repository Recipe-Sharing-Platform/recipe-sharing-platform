using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.DataLayer.Models.DTOs.Review
{
    public class ReviewCreateDTO
    {
        public Guid UserId { get; set; }
        public Guid RecipeId { get; set; }
        public double Rating { get; set; }
        public string? Message { get; set; }
    }
}
