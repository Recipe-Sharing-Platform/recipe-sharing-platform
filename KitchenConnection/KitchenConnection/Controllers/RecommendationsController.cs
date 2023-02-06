using KitchenConnection.BusinessLogic.Helpers.Exceptions.RecommendationExceptions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KitchenConnection.Controllers;

[ApiController]
public class RecommendationsController:ControllerBase
{
    private readonly IRecommendationsService _recommendationService;
    private readonly ILogger<RecommendationsController> _logger;
    public RecommendationsController(IRecommendationsService recommendationsService, ILogger<RecommendationsController> logger)
    {
        _recommendationService = recommendationsService;
        _logger = logger;
    }

    [HttpGet("GetSingleRecommendation")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<Recipe>> GetSingleRecommendation()
    {
        try
        {
            var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var recommendation = await _recommendationService.GetSingleRecommendation(userId);
            
            return Ok(recommendation);
        }
        catch(RecommendationNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(RecipeController)}, Method: {nameof(GetSingleRecommendation)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

    [HttpGet("GetCollectionRecommendation")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<List<Recipe>>> GetCollectionRecommendations(int length)
    {
        try
        {
            var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var recommendationCollection = await _recommendationService.GetCollectionRecommendations(userId, length);

            return Ok(recommendationCollection);
        }
        catch (RecommendationNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(RecipeController)}, Method: {nameof(GetCollectionRecommendations)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
        catch (RecommendationCollectionNotFound ex)
        {
            _logger.LogError($"Error at Class: {nameof(RecipeController)}, Method: {nameof(GetCollectionRecommendations)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

}
