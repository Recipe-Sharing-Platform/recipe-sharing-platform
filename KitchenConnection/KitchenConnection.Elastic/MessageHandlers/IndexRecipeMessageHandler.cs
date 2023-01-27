using KitchenConnection.Elastic.Models;
using KitchenConnection.Models.Entities;
using Nest;

namespace KitchenConnection.Elastic.MessageHandlers;

public class IndexRecipeMessageHandler : IMessageHandler<IndexRecipe>
{
    private readonly IElasticClient _elasticClient;
    public IndexRecipeMessageHandler(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }
    public async Task HandleAsync(IndexRecipe message)
    {
        var indexResponse = await _elasticClient.IndexDocumentAsync(message);
        if (!indexResponse.IsValid)
        {
            throw new Exception("Could not index recipe");
        }
    }
}