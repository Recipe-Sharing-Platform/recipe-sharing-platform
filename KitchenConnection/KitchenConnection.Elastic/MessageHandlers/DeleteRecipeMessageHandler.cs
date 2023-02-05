using KitchenConnection.Elastic.Models;
using KitchenConnection.Models.Entities;
using KitchenConnection.Models.Dispatcher;
using Nest;

namespace KitchenConnection.Elastic.MessageHandlers;
public class DeleteRecipeMessageHandler : IMessageHandler<DeleteRecipe> {
    public readonly IElasticClient _elasticClient;
    public readonly ILogger<DeleteRecipeMessageHandler> _logger;
    public DeleteRecipeMessageHandler(IElasticClient elasticClient, ILogger<DeleteRecipeMessageHandler> logger) {
        _elasticClient = elasticClient;
        _logger = logger;
    }

    public async Task HandleAsync(DeleteRecipe message) {
        // delete the recipe from the recipe index by id
        var deleteResponse = await _elasticClient.DeleteAsync<Recipe>(message.RecipeId);
        if (deleteResponse.IsValid) {
            _logger.LogInformation($"Deleted recipe with id {message.RecipeId}", deleteResponse);
        } else {
            _logger.LogError($"Failed to delete recipe with id {message.RecipeId}", deleteResponse);
        }
    }
}
