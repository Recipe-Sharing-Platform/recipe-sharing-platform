using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.DataLayer.Models.DTOs
{
    public class RecommendationScoreCreateDto
    {
        public Guid UserId { get; set; }
        public Guid TagId { get; set; }
        public int Score { get; set; }
    }
}
