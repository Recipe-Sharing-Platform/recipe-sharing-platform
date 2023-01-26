using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KitchenConnection.Controllers;

[ApiController]
[Route("api/recipes")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;
    public RecipeController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [HttpPost]
    public async Task<ActionResult<RecipeDTO>> Create(RecipeCreateDTO recipeToCreate)
    {
        recipeToCreate.UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var recipe = await _recipeService.Create(recipeToCreate);

        if(recipe == null)
        {
            return BadRequest("Recipe could not be created!");
        }

        return Ok(recipe);
    }

    [HttpGet]
    public async Task<ActionResult<List<RecipeDTO>>> GetAll()
    {
        var recipes = await _recipeService.GetAll();

        if(recipes == null)
        {
            return NotFound();
        }

        return Ok(recipes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeDTO>> Get(Guid id)
    {
        var recipe = await _recipeService.Get(id);
        
        if (recipe == null)
        {
            return NotFound();
        }
        
        return Ok(recipe);
    }

    [HttpPut]
    public async Task<ActionResult<RecipeDTO>> Update(RecipeUpdateDTO recipeToUpdate)
    {
        var recipe = await _recipeService.Update(recipeToUpdate);

        if(recipe == null)
        {
            return BadRequest("Recipe could not be updated!");
        }

        return Ok(recipe);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<RecipeDTO>> Delete(Guid id)
    {
        var recipe = await _recipeService.Delete(id);

        if(recipe == null)
        {
            return BadRequest("Recipe could not be deleted!");
        }

        return Ok(recipe);
    }
}
