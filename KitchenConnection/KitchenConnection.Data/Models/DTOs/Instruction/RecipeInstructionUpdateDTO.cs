using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.DataLayer.Models.DTOs.Instruction
{
    public class RecipeInstructionUpdateDTO
    {
        public Guid Id { get; set; }
        public int StepNumber { get; set; }
        public string StepDescription { get; set; }
    }
}
