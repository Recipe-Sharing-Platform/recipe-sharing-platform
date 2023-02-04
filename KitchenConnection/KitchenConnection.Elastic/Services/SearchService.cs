using KitchenConnection.Elastic.Services.IServices;
using KitchenConnection.Models.Entities;
using Nest;

namespace KitchenConnection.Elastic.Services;
public class SearchService : ISearchService {
    public readonly IElasticClient _elasticClient;
    public readonly ILogger<SearchService> _logger;

    public SearchService(IElasticClient elasticClient, ILogger<SearchService> logger) {
        _elasticClient = elasticClient;
        _logger = logger;
    }

    public async Task<List<Recipe>> SearchRecipes(string? query) {

        var searchResponse = await _elasticClient.SearchAsync<Recipe>(s => s
            .Index("recipe")
            .Query(q => q
                .FunctionScore(fs => fs
                    .Query(fq => fq
                        .MultiMatch(mm => mm
                            .Fields(f => f
                                .Field(p => p.Name)
                                .Field(p => p.Description)
                            )
                            .Query(query ?? "")
                            .Fuzziness(Fuzziness.Auto)
                            .Fuzziness(Fuzziness.EditDistance(3))
                        )
                    )
                    .Functions(f => f
                        .Weight(w => w
                            .Weight(2)
                        )
                    )
                )
            ));

        // var mappingResponse = _elasticClient.Indices.GetMapping(new GetMappingRequest("recipe"));


        return searchResponse.Documents.ToList();

    }

    public async Task<List<Recipe>> SearchRecipesWithAnotherQuery(string query) {

        var searchResponse = await _elasticClient.SearchAsync<Recipe>(s => s
            .Index("recipe")
            .Query(q => q
                .MultiMatch(mm => mm
                    .Fields(f => f
                        .Field(p => p.User.FirstName, 1.6)
                        .Field(p => p.User.LastName, 1.6)
                        .Field(p => p.Name, 1.2)
                        .Field(p => p.Description)
                    )
                    .Query(query)
                    .TieBreaker(0.3)
                    .Fuzziness(Fuzziness.EditDistance(3))
                )
            ));


        // var mappingResponse = _elasticClient.Indices.GetMapping(new GetMappingRequest("recipe"));

        return searchResponse.Documents.ToList();

    }
}
