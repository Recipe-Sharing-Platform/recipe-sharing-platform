using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.Models.DTOs.Nutrients
{
    public class NutrientsResponseDTO
    {
        public string Name { get; set; }
        public double Calories { get; set; }
        public double Serving_Size_G { get; set; }
        public double Fat_Total_G { get; set; }
        public double Protein_G { get; set; }
        public double Sodium_Mg { get; set; }
        public double Potassium_Mg { get; set; }
        public double Cholesterol_Mg { get; set; }
        public double Carbohydrates_Total_G { get; set; }
        public double Fiber_G { get; set; }
        public double Sugar_G { get; set; }
    }
}
