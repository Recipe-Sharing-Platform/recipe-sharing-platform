using KitchenConnection.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.DataLayer.Models.Entities.Mappings;

public class RecipeInstruction : BaseEntity
{
    public Guid RecipeId { get; set; } // Recipe that this step corresponds to
    public Recipe Recipe { get; set; }
    public int StepNumber { get; set; }
    public string StepDescription { get; set; }
}
