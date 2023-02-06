namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.RecipeExceptions {
    public class RecipeNotFoundException : Exception
    {
        public RecipeNotFoundException(Guid recipeId) : base($"Recipe not found: {recipeId}") {}

        public RecipeNotFoundException(string message) : base(message) {}
    }

    public class RecipesNotFoundException : Exception
    {
        public RecipesNotFoundException() : base("No recipes were found!") { }

        public RecipesNotFoundException(string message) : base(message) { }
    }
}
