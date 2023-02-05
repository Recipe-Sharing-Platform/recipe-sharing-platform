namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.RecipeExceptions {
    public class RecipeNutrientsNotFoundException : Exception
    {
        public RecipeNutrientsNotFoundException(Guid? recipeId) : base($"Could not find nutrients for Recipe: {recipeId}"){}
    }
}
