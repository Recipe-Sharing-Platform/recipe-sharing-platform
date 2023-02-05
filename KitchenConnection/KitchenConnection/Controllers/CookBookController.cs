using KitchenConnection.BusinessLogic.Helpers.Exceptions.CollectionExceptions;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.CookBookExceptions;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.RecipeExceptions;
using KitchenConnection.BusinessLogic.Helpers.Exceptions.CookBookExceptions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.Models.DTOs.CookBook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KitchenConnection.Controllers;
[ApiController]
[Route("api/cookbooks")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CookBookController : ControllerBase {
    private readonly ICookBookService _cookBookService;
    private readonly ILogger<RecipeController> _logger;


    public CookBookController(ICookBookService cookBookService, ILogger<RecipeController> logger) {
        _cookBookService = cookBookService;
        _logger = logger;

    }

    [HttpPost]
    public async Task<ActionResult<CookBookDTO>> Create(CookBookCreateRequestDTO cookBookToCreate) {
        try {
            var UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            CookBookDTO cookBook;
            try {
                cookBook = await _cookBookService.Create(cookBookToCreate, UserId);
            } catch (CookBookEmptyException ex) {
                _logger.LogError($"Error at Class: {nameof(CookBookController)}, Method: {nameof(Create)}, Exception: {ex}");
                return BadRequest(ex.Message);
            }
            return Ok(cookBook);
        } catch (CookBookCouldNotBeCreatedException ex) {
            _logger.LogError($"Error at Class: {nameof(CookBookController)}, Method: {nameof(Create)}, Exception: {ex}");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<CookBookDTO>>> GetAll() {
        try {
            var cookBooks = await _cookBookService.GetAll();
            return Ok(cookBooks);
        } catch (CollectionsNotFoundException ex) {
            _logger.LogError($"Error at Class: {nameof(CookBookController)}, Method: {nameof(Create)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CookBookDTO>> Get(Guid id) {
        try {
            var cookBook = await _cookBookService.Get(id);
            return Ok(cookBook);
        } catch (CookBookNotFoundException ex) {
            _logger.LogError($"Error at Class: {nameof(CookBookController)}, Method: {nameof(Create)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult<CookBookDTO>> UpdateCookBook(CookBookUpdateDTO cookBookToUpdate) {
        try {
            var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var cookBook = await _cookBookService.Update(cookBookToUpdate, userId);
            return Ok(cookBook);
        } catch (CookBookNotFoundException ex) {
            _logger.LogError($"Error at Class: {nameof(CookBookController)}, Method: {nameof(Create)}, Exception: {ex}");
            return NotFound(ex.Message);

        } catch (CookBookCouldNotBeUpdatedException ex) {
            _logger.LogError($"Error at Class: {nameof(CookBookController)}, Method: {nameof(Create)}, Exception: {ex}");
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id) {
        var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        CookBookDTO cookBook;
        try {
            cookBook = await _cookBookService.Delete(id, userId);
        } catch (CookBookNotFoundException ex) {
            _logger.LogError($"Error at Class: {nameof(CookBookController)}, Method: {nameof(Delete)}, Exception: {ex}");
            return NotFound(ex.Message);
        }

        return Ok(cookBook);
    }
}
