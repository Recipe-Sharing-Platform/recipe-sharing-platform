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
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<List<CollectionDTO>>> GetAll()
    {
        try
        {
            var UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return await _collectionService.GetAll(UserId);
        }
        catch (CollectionsNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(CollectionController)}, Method: {nameof(GetAll)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
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
            _logger.LogError($"Error at Class: {nameof(CollectionController)}, Method: {nameof(Get)}, Exception: {ex}");
            return NotFound(ex.Message);
        }

    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<List<CollectionDTO>>> GetPaginated(int page, int pageSize)
    {
        var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        List<CollectionDTO> collections = await _collectionService.GetPaginated(page, pageSize, userId);

        return collections;
    }
    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer")]
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
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<CollectionDTO>> UpdateCollection(CollectionUpdateDTO collectionToUpdate)
    {
        try
        {
            var UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var collection = await _collectionService.Update(UserId, collectionToUpdate);
            return Ok(collection);
        }
        catch (RecipeNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(CookBookController)}, Method: {nameof(Create)}, Exception: {ex}");
            return BadRequest(ex.Message);
        }
        catch (CollectionNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(CollectionController)}, Method: {nameof(UpdateCollection)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
        catch (CollectionCouldNotBeUpdatedException ex)
        {
            _logger.LogError($"Error at Class: {nameof(CollectionController)}, Method: {nameof(UpdateCollection)}, Exception: {ex}");
            return BadRequest(ex.Message);
        }
    }


    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
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
            _logger.LogError($"Error at Class: {nameof(CollectionController)}, Method: {nameof(Delete)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

}
