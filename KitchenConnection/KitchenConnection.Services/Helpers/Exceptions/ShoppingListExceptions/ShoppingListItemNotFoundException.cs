namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.ShoppingListExceptions
{
    public class ShoppingListItemNotFoundException : Exception
    {
        public ShoppingListItemNotFoundException(Guid shoppingListItemId) : base($"Shopping list item not found: {shoppingListItemId}") {}

        public ShoppingListItemNotFoundException(string message) : base(message) {}
    }
    public class ShoppingListItemsNotFoundException : Exception
    {
        public ShoppingListItemsNotFoundException() : base("No items in shopping list were found!") { }
    }
}
