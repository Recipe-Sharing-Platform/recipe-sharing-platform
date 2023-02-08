using KitchenConnection.BusinessLogic.Helpers.Exceptions.ShoppingListExceptions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.Models.DTOs.ShoppingCart;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KitchenConnection.Controllers;

[ApiController]
[Route("api/ShoppingList")]
public class ShoppingListController : ControllerBase
{
    private readonly IShoppingListService _shoppingListService;
    public ShoppingListController(IShoppingListService shoppingListService)
    {
        _shoppingListService = shoppingListService;
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<List<ShoppingListItemCreateDTO>>> AddToShoppingList(List<ShoppingListItemCreateDTO> shoppingListToCreate)
    {
        var UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var shoppingList = await _shoppingListService.AddToShoppingList(UserId, shoppingListToCreate);
            return Ok(shoppingList);
        }
        catch(ShoppingListItemCouldNotBeAddedException ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpDelete]
    [Route("{itemId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<bool>> DeleteShoppingListItems(Guid itemId)
    {
        var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var deletedItems = await _shoppingListService.DeleteFromShoppingList(userId, itemId);
            return Ok(deletedItems);
        }
        catch(ShoppingListItemNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch(ShoppingListItemCouldNotBeDeletedException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("{shoppingListItemId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<ShoppingListItem>> GetShoppingListItem(Guid shoppingListItemId)
    {
        var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var shoppingListItem = await _shoppingListService.GetShoppingListItemById(userId, shoppingListItemId);
            return Ok(shoppingListItem);
        }
        catch(ShoppingListItemNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    [Route("getlink/{shoppingListItemId}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetShoppingListItemLink(Guid shoppingListItemId)
    {
        var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var finalUrl = await _shoppingListService.GetShoppingListItemUrl(userId, shoppingListItemId);
            return Redirect(finalUrl);
        }
        catch (ShoppingListItemNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<List<ShoppingListItem>>> GetShoppingList()
    {
        var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var shoppingList = await _shoppingListService.GetShoppingListForUser(userId);
            return Ok(shoppingList);
        }
        catch (ShoppingListItemsNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}