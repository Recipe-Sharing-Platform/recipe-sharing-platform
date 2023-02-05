using KitchenConnection.Models.Dispatcher;
using KitchenConnection.Elastic.Models;
using KitchenConnection.Models.Entities;
using Nest;

namespace KitchenConnection.Elastic.MessageHandlers;
public class UpdateRecipeMessageHandler : IMessageHandler<UpdateRecipe> {
    private readonly IElasticClient _elasticClient;
    private readonly ILogger<UpdateRecipeMessageHandler> _logger;

    public UpdateRecipeMessageHandler(IElasticClient elasticClient, ILogger<UpdateRecipeMessageHandler> logger) {
        _elasticClient = elasticClient;
        _logger = logger;
    }

    public async Task HandleAsync(UpdateRecipe message) {
        // update the recipe in the recipe index by id
        var updateResponse = await _elasticClient.UpdateAsync<Recipe>(message.Id, u => u.Doc(message));
        if (updateResponse.IsValid) {
            _logger.LogInformation($"Updated recipe with id {message.Id}", updateResponse);
        } else {
            _logger.LogError($"Failed to update recipe with id {message.Id}", updateResponse);
        }
    }
}