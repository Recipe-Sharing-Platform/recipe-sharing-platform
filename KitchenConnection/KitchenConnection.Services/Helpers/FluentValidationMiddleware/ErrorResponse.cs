namespace KitchenConnection.BusinessLogic.Helpers.FluentValidationMiddleware {
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}
