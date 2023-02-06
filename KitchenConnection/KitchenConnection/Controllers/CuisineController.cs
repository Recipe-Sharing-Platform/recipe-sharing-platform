using KitchenConnection.BusinessLogic.Helpers.Exceptions.CuisineExceptions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.Models.DTOs.Cuisine;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KitchenConnection.Controllers;

[ApiController]
[Route("api/cuisines")]
public class CuisineController : ControllerBase
{
    private readonly ICuisineService _cuisineService;
    private readonly ILogger<CuisineController> _logger;

    public CuisineController(ICuisineService cuisineService, ILogger<CuisineController> logger)
    {
        _cuisineService = cuisineService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "RSP_Admin")]
    public async Task<ActionResult<CuisineDTO>> Create(Cuisine cuisineToCreate)
    {
        try
        {
            var cuisine = await _cuisineService.Create(cuisineToCreate);
            return Ok(cuisine);
        }
        catch(CuisineCouldNotBeCreatedException ex)
        {
            _logger.LogError($"Error at Class: {nameof(CuisineController)}, Method: {nameof(Create)}, Exception: {ex}");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<CuisineDTO>>> GetAll()
    {

        try
        {
            var cuisines = await _cuisineService.GetAll();

            return Ok(cuisines);
        }
        catch (CuisinesNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(CuisineController)}, Method: {nameof(GetAll)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
        
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CuisineDTO>> Get(Guid cuisineId)
    {
        try
        {
            var cuisine = await _cuisineService.Get(cuisineId);

            return Ok(cuisine);
        }
        catch (CuisineNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(CuisineController)}, Method: {nameof(Get)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<CuisineDTO>>> GetPaginated(int page, int size)
    {
        var cuisines = await _cuisineService.GetPaginated(page, size);

        return Ok(cuisines);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "RSP_Admin")]
    public async Task<ActionResult<CuisineDTO>> Update(CuisineUpdateDTO cuisineToUpdate)
    {
        try
        {
            var cuisine = await _cuisineService.Update(cuisineToUpdate);
            return Ok(cuisine);
        }
        catch (CuisineNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(CuisineController)}, Method: {nameof(Update)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
        catch (CuisineCouldNotBeUpdatedException ex)
        {
            _logger.LogError($"Error at Class: {nameof(CuisineController)}, Method: {nameof(Update)}, Exception: {ex}");
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "RSP_Admin")]
    public async Task<ActionResult<CuisineDTO>> Delete(Guid id)
    {
        try
        {
            var cuisine = await _cuisineService.Delete(id);
            return Ok(cuisine);
        }
        catch (CuisineNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(CuisineController)}, Method: {nameof(Delete)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

}
