using KitchenConnection.Elastic.Services.IServices;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace KitchenConnection.Elastic.Controllers;

[ApiController]
[Route("search")]
public class SearchController : ControllerBase {
	public readonly ISearchService _searchService;
    public SearchController(ISearchService searchService) {
        _searchService = searchService;
    }

    [HttpGet]
    [Route("searchOnName")]
    public async Task<ActionResult<List<Recipe>>> SearchRecipes(string query) {
        var recipesFound = await _searchService.SearchRecipes(query);
        return Ok(recipesFound);
    }

    [HttpGet]
    [Route("searchWithUser")]
    public async Task<ActionResult<List<Recipe>>> SearchRecipesWithUser(string query) {
        var recipesFound = await _searchService.SearchRecipesWithAnotherQuery(query);
        return Ok(recipesFound);
    }
}