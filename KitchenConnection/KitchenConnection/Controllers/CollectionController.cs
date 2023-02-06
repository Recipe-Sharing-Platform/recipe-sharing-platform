using KitchenConnection.BusinessLogic.Helpers.Exceptions.CollectionExceptions;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.RecipeExceptions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.Models.DTOs.Collection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KitchenConnection.Controllers;

[ApiController]
[Route("api/collections")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CollectionController : ControllerBase
{
    private readonly ICollectionService _collectionService;
    private readonly ILogger<RecipeController> _logger;


    public CollectionController(ICollectionService collectionService, ILogger<RecipeController> logger)
    {
        _collectionService = collectionService;
        _logger = logger;

    }

    [HttpGet]
    public async Task<ActionResult<List<CollectionDTO>>> GetAll()
    {
        try
        {
            var UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return await _collectionService.GetAll(UserId);
        }
        catch (CollectionsNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(CollectionController)}, Method: {nameof(Create)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CollectionDTO>> Get(Guid id)
    {
        try
        {
            var UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var collection = await _collectionService.Get(UserId, id);
            return Ok(collection);
        }
        catch (CollectionNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(CollectionController)}, Method: {nameof(Create)}, Exception: {ex}");
            return NotFound(ex.Message);
        }

    }
    [HttpPost]
    public async Task<ActionResult<CollectionDTO>> Create(CollectionCreateRequestDTO collectionToCreate)
    {
        try
        {
            var UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var collection = await _collectionService.Create(UserId, collectionToCreate);

            if (collection == null) return NotFound();

            return Ok(collection);
        }
        catch (CollectionCouldNotBeCreatedException ex)
        {
            _logger.LogError($"Error at Class: {nameof(CollectionController)}, Method: {nameof(Create)}, Exception: {ex}");
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult<CollectionDTO>> UpdateCollection(CollectionUpdateDTO collectionToUpdate)
    {
        try
        {
            var UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var collection = await _collectionService.Update(UserId, collectionToUpdate);
            return Ok(collection);
        }
        catch (CollectionNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(CollectionController)}, Method: {nameof(Create)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
        catch (CollectionCouldNotBeUpdatedException ex)
        {
            _logger.LogError($"Error at Class: {nameof(CollectionController)}, Method: {nameof(Create)}, Exception: {ex}");
            return BadRequest(ex.Message);
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        try
        {
            var collection = await _collectionService.Delete(UserId, id);
            return Ok(collection);
        }
        catch (CollectionsNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(CollectionController)}, Method: {nameof(Create)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

}
