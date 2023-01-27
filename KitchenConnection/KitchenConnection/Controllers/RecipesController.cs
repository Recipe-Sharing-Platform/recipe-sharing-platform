using KitchenConnection.Application.Models.DTOs.Recipe;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenConnection.Controllers
{
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        public RecipesController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet("GetAllRecipes")]
        public async Task<IActionResult> GetRecipes()
        {
            var recipes = await _recipeService.GetRecipes();

            if (recipes != null)
            {
                return Ok(recipes);
            }
            else
            {
                return NotFound("Could not find recipes!");
            }
            
        }
        [HttpGet("GetSingleRecipe")]
        public async Task<IActionResult> GetRecipe(Guid id)
        {
            var Recipe = await _recipeService.GetRecipe(id);
            if (Recipe == null)
            {
                return NotFound("Could not find recipe!");
            }
            return Ok(Recipe);
        }

        [HttpPost("CreateRecipe")]
        public async Task<IActionResult> CreateRecipe(RecipeCreateDTO RecipeToCreate)
        {
            var recipe=await _recipeService.CreateRecipe(RecipeToCreate);

            if (recipe != null)
            {
                return Ok(recipe);
            }

            else
            {
                return BadRequest("Could not create recipe!");
            }
            
        }
        [HttpPut("UpdateRecipe")]
        public async Task<IActionResult> UpdateRecipe(RecipeDTO recipe)
        {
            var updatedRecipe= await _recipeService.UpdateRecipe(recipe);
            if (updatedRecipe != null)
            {
                return Ok("Recipe updated Successfully!");
            }

            else
            {
                return BadRequest("Could not update recipe!");
            }
        }
        [HttpDelete("DeleteRecipe")]
        public async Task<IActionResult> DeleteRecipe(Guid id)
        {
            var res= await _recipeService.DeleteRecipe(id);

            if (res)
            {
                return Ok("Recipe Deleted Successfully!");
            }

            else
            {
                return BadRequest("Could not delete recipe!");
            }
            
        }
    }
}
