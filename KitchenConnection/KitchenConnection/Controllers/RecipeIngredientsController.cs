using KitchenConnection.Application.Models.DTOs.Recipe;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Models.Entities.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace KitchenConnection.Controllers
{

    [ApiController]
    public class RecipeIngredientsController : ControllerBase
    {

        private readonly IRecipeIngredientService _recipeIngredientService;
        public RecipeIngredientsController(IRecipeIngredientService recipeIngredientService)
        {
            _recipeIngredientService = recipeIngredientService;
        }

        [HttpGet("GetAllRecipeIngredients")]
        public async Task<IActionResult> GetRecipeIngredients()
        {
            var recipes = await _recipeIngredientService.GetRecipeIngredients();

            if (recipes != null)
            {
                return Ok(recipes);
            }
            else
            {
                return NotFound("Requested recipes were not found!");
            }
        }
        [HttpGet("GetSingleRecipeIngredient")]
        public async Task<IActionResult> GetRecipeIngredient(Guid id)
        {
            var recipe = await _recipeIngredientService.GetRecipeIngredient(id);
            if (recipe == null)
            {
                return NotFound("Requested recipe ingredient wasn't found!");
            }
            return Ok(recipe);
        }

        [HttpPost("CreateRecipeIngredients")]
        public async Task<IActionResult> CreateRecipe(RecipeIngredientCreateDTO recipeIngredientToCreate)
        {
            var createdRecipeIngredient = await _recipeIngredientService.CreateRecipeIngredient(recipeIngredientToCreate);

            if (createdRecipeIngredient != null)
            {
                return Ok(createdRecipeIngredient);
            }
            else
            {
                return BadRequest("Recipe ingredient was not created!");
            }

        }
        [HttpPut("UpdateRecipeingredient")]
        public async Task<IActionResult> UpdateRecipe(RecipeIngredientDTO recipeIngredient)
        {
            var recipeIngredientUpdated = await _recipeIngredientService.UpdateRecipeIngredient(recipeIngredient);

            if (recipeIngredientUpdated != null)
            {
                return Ok(recipeIngredientUpdated);
            }
            else
            {
                return BadRequest("Was not updated!");
            }

        }
        [HttpDelete("DeleteRecipeIngredient")]
        public async Task<IActionResult> DeleteRecipeIngredient(Guid id)
        {
            var res = await _recipeIngredientService.DeleteRecipeIngredient(id);

            if (res)
            {
                return Ok("Recipe Deleted Successfully!");
            }
            else
            {
                return BadRequest("Was not deleted!");
            }
        }
    }
}
