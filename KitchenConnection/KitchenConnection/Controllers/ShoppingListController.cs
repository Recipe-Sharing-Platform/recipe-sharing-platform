﻿using KitchenConnection.BusinessLogic.Services;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using KitchenConnection.DataLayer.Models.DTOs.ShoppingCart;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KitchenConnection.Controllers
{
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
        public async Task<ActionResult<RecipeDTO>> AddToShoppingList(List<ShoppingListItemCreateDTO> shoppingListToCreate)
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
        public async Task<ActionResult<List<ShoppingListItemCreateDTO>>> DeleteShoppingListItems(Guid itemId)
        {
            var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var deletedItems = await _shoppingListService.DeleteFromShoppingList(userId, itemId);

            if (deletedItems == false)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet]
        [Route("{shoppingListItemId}")]
        public async Task<ActionResult<ShoppingListItemDTO>> GetShoppingListItem(Guid shoppingListItemId)
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
        public async Task<ActionResult<List<ShoppingListItemDTO>>> GetShoppingList()
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
