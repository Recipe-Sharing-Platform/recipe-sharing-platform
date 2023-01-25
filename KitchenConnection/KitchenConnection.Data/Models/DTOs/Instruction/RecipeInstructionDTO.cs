namespace KitchenConnection.DataLayer.Models.DTOs.Recipe
{
    public class RecipeInstructionDTO
    {
        public Guid Id { get; set; }
        public int StepNumber { get; set; }
        public string StepDescription { get; set; }
    }
}
