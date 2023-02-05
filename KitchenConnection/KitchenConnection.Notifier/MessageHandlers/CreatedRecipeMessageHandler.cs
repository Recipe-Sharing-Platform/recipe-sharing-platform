using KitchenConnection.Models.Dispatcher;
using KitchenConnection.Notifier.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace KitchenConnection.Notifier.MessageHandlers;
public class CreatedRecipeMessageHandler : IMessageHandler<CreatedRecipe> {
    private readonly ILogger<CreatedRecipeMessageHandler> _logger;
    private readonly IEmailSender _emailSender;

    public CreatedRecipeMessageHandler(ILogger<CreatedRecipeMessageHandler> logger, IEmailSender emailSender) {
        _logger = logger;
        _emailSender = emailSender;
    }
    public Task HandleAsync(CreatedRecipe message) {
        var htmlMessage = $@"
            <h1>New Recipe Created</h1>
            <p>Recipe {message.Name} was created</p>
        ";
        _logger.LogInformation(string.Format("Sending email to {0} with message {1}", message.User.Email, htmlMessage), message.User.Email, htmlMessage);
        return _emailSender.SendEmailAsync(message.User.Email, "New Recipe Successfully Created", htmlMessage);
    }
}
