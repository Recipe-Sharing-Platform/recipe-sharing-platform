using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KitchenConnection.Controllers
{
    [ApiController]
    [Route("recipes")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecipeCreateDTO recipeToCreate)
        {
            recipeToCreate.UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _recipeService.Create(recipeToCreate);

            return Ok("Recipe created successfully!");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var recipes = await _recipeService.GetAll();
            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var Recipe = await _recipeService.Get(id);
            if (Recipe == null)
            {
                return NotFound();
            }
            return Ok(Recipe);
        }

        [HttpPut]
        public async Task<IActionResult> Update(RecipeUpdateDTO recipe)
        {
            await _recipeService.Update(recipe);
            return Ok("Recipe updated Successfully!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _recipeService.Delete(id);
            return Ok("Recipe Deleted Successfully!");
        }
    }
}
