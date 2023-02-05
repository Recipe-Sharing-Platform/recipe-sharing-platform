using KitchenConnection.Models.Dispatcher;
using KitchenConnection.Notifier.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace KitchenConnection.Notifier.MessageHandlers;
public class CreatedReviewMessageHandler : IMessageHandler<CreatedReview> {
    private readonly ILogger<CreatedReviewMessageHandler> _logger;
    private readonly IEmailSender _emailSender;

    public CreatedReviewMessageHandler(ILogger<CreatedReviewMessageHandler> logger, IEmailSender emailSender) {
        _logger = logger;
        _emailSender = emailSender;
    }

    public async Task HandleAsync(CreatedReview message) {
        // send email to user that created the recipe
        var htmlMessage = @"
            <h1>A New Review has been created on Recipe {0}</h1>
            <p><strong>{1}</strong> gave {2} stars on your Recipe</p>
            <p>{1} : '{3}'</p>
        ";
        var formattedHtmlMessage = string.Format(htmlMessage, message.Recipe.Name, $"{message.User.FirstName} {message.User.LastName}", message.Review.Rating, message.Review.Message);
        _logger.LogInformation($"Sending email to {message.User.Email} with message: {formattedHtmlMessage}");
        await _emailSender.SendEmailAsync(message.User.Email, "A new review was left on your Recipe", formattedHtmlMessage);        
    }
}
