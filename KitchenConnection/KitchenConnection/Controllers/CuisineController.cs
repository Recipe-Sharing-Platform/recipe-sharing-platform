using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KitchenConnection.Controllers;

[ApiController]
[Route("api/cuisines")]
[Authorize(AuthenticationSchemes = "Bearer", Roles = "RSP_Admin")]
public class CuisineController : ControllerBase
{
    private readonly ICuisineService _cuisineService;

    public CuisineController(ICuisineService cuisineService)
    {
        _cuisineService = cuisineService;
    }

    [HttpPost]
    public async Task<Cuisine> Create(Cuisine cuisineToCreate)
    {
        var cuisine = await _cuisineService.Create(cuisineToCreate);
        return cuisine;
    }
}
