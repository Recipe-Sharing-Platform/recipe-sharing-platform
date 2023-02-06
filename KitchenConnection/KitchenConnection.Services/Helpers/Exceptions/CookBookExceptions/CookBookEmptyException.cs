namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.CookBookExceptions; 
public class CookBookEmptyException : Exception {
	public CookBookEmptyException() { }
	public CookBookEmptyException(string message) : base(message) { }
}
