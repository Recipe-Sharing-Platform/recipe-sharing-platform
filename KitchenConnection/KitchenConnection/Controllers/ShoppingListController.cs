using KitchenConnection.BusinessLogic.Services;
using KitchenConnection.BusinessLogic.Services.IServices;
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
        public async Task<ActionResult<ShoppingList>> CreateShopingList(ShoppingListCreateDTO shoppingListToCreate)
        {
            var UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var shoppingList = await _shoppingListService.AddShoppingList(UserId, shoppingListToCreate);

            if (shoppingList == null)
            {
                return BadRequest("Shopping List could not be added");
            }

            return Ok(shoppingList);
        }
    }
}
