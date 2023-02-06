using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.Models.Entities.Mappings;

public class RecipeNutrients : BaseEntity
{
    public Guid RecipeId { get; set; }
    public Recipe Recipe { get; set; }
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Fiber { get; set; }
    public double VitaminC { get; set; }
    public double Sodium { get; set; }
    public double Potassium { get; set; }
    public double TotalFat { get; set; }
    public double TotalCarbohydrates { get; set; }
    public double TotalSugar { get; set; }
}
