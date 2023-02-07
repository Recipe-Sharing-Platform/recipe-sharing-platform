using KitchenConnection.BusinessLogic.Helpers.Exceptions.RecipeExceptions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.Models.DTOs.Recipe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KitchenConnection.Controllers;

[ApiController]
[Route("api/recipes")]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;
    private readonly ILogger<RecipeController> _logger;
    public RecipeController(IRecipeService recipeService, ILogger<RecipeController> logger)
    {
        _recipeService = recipeService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<RecipeDTO>> Create(RecipeCreateDTO recipeToCreate)
    {
        try
        {
            var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var recipe = await _recipeService.Create(userId, recipeToCreate);

            return Ok(recipe);
        }
        catch(RecipeCouldNotBeCreatedException ex)
        {
            _logger.LogError($"Error at Class: {nameof(RecipeController)}, Method: {nameof(Create)}, Exception: {ex}");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet] 
    public async Task<ActionResult<List<RecipeDTO>>> GetPaginated(int page, int pageSize) {
        List<RecipeDTO> recipes = await _recipeService.GetPaginated(page, pageSize);

        return recipes;
    }

    [HttpGet]
    [Route("getAll")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "RSP_Admin")]
    public async Task<ActionResult<List<RecipeDTO>>> GetAll()
    {
        try
        {
            var recipes = await _recipeService.GetAll();

            return Ok(recipes);
        }
        catch(RecipesNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(RecipeController)}, Method: {nameof(GetAll)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeDTO>> Get(Guid id)
    {
        try
        {
            Guid? userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            var recipe = await _recipeService.Get(id,userId);

            return Ok(recipe);
        }
        catch (RecipeNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(RecipeController)}, Method: {nameof(Get)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

   [HttpGet("{id}/nutrients")]
   public async Task<IActionResult> GetRecipeNutrients(Guid id)
    {
        try
        {
            var result = await _recipeService.GetRecipeNutrients(id);

            return Ok(result);
        }
        catch(RecipeNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(RecipeController)}, Method: {nameof(GetRecipeNutrients)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
        catch (RecipeNutrientsNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(RecipeController)}, Method: {nameof(GetRecipeNutrients)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<RecipeDTO>> Update(RecipeUpdateDTO recipeToUpdate)
    {
        try
        {
            var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var recipe = await _recipeService.Update(recipeToUpdate, userId);

            return Ok(recipe);
        }
        catch (RecipeCouldNotBeUpdatedException ex)
        {
            _logger.LogError($"Error at Class: {nameof(RecipeController)}, Method: {nameof(Update)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<RecipeDTO>> Delete(Guid id)
    {
        try
        {
            var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var recipe = await _recipeService.Delete(id, userId);

            return Ok(recipe);
        }
        catch (RecipeNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(RecipeController)}, Method: {nameof(Delete)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

}
