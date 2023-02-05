namespace KitchenConnection.Models.DTOs.Instruction
{
    public class RecipeInstructionDTO
    {
        public Guid Id { get; set; }
        public int StepNumber { get; set; }
        public string StepDescription { get; set; }
    }
}
