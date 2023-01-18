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
            var Recipes = await _recipeService.GetRecipes();
            return Ok(Recipes);
        }
        [HttpGet("GetSingleRecipe")]
        public async Task<IActionResult> GetRecipe(string id)
        {
            var Recipe = await _recipeService.GetRecipe(id);
            if (Recipe == null)
            {
                return NotFound();
            }
            return Ok(Recipe);
        }

        [HttpPost("CreateRecipe")]
        public async Task<IActionResult> CreateRecipe(Recipe RecipeToCreate)
        {
            await _recipeService.CreateRecipe(RecipeToCreate);

            return Ok("Recipe created successfully!");
        }
        [HttpPut("UpdateRecipe")]
        public async Task<IActionResult> UpdateRecipe(Recipe recipe)
        {
            await _recipeService.UpdateRecipe(recipe);
            return Ok("Recipe updated Successfully!");
        }
        [HttpDelete("DeleteRecipe")]
        public async Task<IActionResult> DeleteRecipe(string id)
        {
            await _recipeService.DeleteRecipe(id);
            return Ok("Recipe Deleted Successfully!");
        }
    }
}
