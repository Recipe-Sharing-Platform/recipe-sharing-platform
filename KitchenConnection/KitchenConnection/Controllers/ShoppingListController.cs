using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.Models.DTOs.Recipe;
using KitchenConnection.Models.DTOs.ShoppingCart;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KitchenConnection.Controllers {
    [ApiController]
    [Route("api/ShoppingList")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;
        public ShoppingListController(IShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }

        [HttpPost]
        [Route("shoppinglist")]
        public async Task<ActionResult<List<ShoppingListItemCreateDTO>>> AddToShoppingList(List<ShoppingListItemCreateDTO> shoppingListToCreate)
        {
            var UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var shoppingList = await _shoppingListService.AddToShoppingList(UserId, shoppingListToCreate);

            if (shoppingList == null)
            {
                return BadRequest("Items could not be added");
            }

            return Ok(shoppingList);
        }
        [HttpDelete]
        [Route("")]
        public async Task<ActionResult<bool>> DeleteShoppingListItems(Guid itemId)
        {
            var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var deletedItems = await _shoppingListService.DeleteFromShoppingList(userId, itemId);

            if (deletedItems == false)
            {
                return NotFound();
            }

            return Ok(deletedItems);
        }

        [HttpGet]
        [Route("{shoppingListItemId}")]
        public async Task<ActionResult<ShoppingListItem>> GetShoppingListItem(Guid shoppingListItemId)
        {
            var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var shoppingListItem = await _shoppingListService.GetShoppingListItemById(userId, shoppingListItemId);

            if (shoppingListItem == null)
            {
                return NotFound();
            }

            return Ok(shoppingListItem);
        }

        [HttpGet]
        [Route("getlink/{shoppingListItemId}")]
        public async Task<IActionResult> GetShoppingListItemLink(Guid shoppingListItemId)
        {
            var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var finalUrl = await _shoppingListService.GetShoppingListItemUrl(userId, shoppingListItemId);
                return Redirect(finalUrl);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<ShoppingListItem>>> GetShoppingList()
        {
            var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var shoppingList = await _shoppingListService.GetShoppingListForUser(userId);

            if (shoppingList == null)
            {
                return NotFound();
            }

            return Ok(shoppingList);
        }
    }
}