using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.DataLayer.Models.DTOs.Review
{
    public class ReviewUpdateDTO
    {
        public Guid Id { get; set; }
        public double Rating { get; set; }
        public string Message { get; set; }
    }
}
