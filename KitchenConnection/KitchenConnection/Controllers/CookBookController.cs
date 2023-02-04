using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Models.DTOs.CookBook;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KitchenConnection.Controllers;
[ApiController]
[Route("api/cookbooks")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CookBookController : ControllerBase
{
    private readonly ICookBookService _cookBookService;

    public CookBookController(ICookBookService cookBookService)
    {
        _cookBookService = cookBookService;
    }

    [HttpPost]
    public async Task<ActionResult<CookBookDTO>> Create(CookBookCreateRequestDTO cookBookToCreate)
    {
        var UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var cookBook = await _cookBookService.Create(cookBookToCreate, UserId);

        if (cookBook == null) return NotFound();

        return Ok(cookBook);
    }

    [HttpGet]
    public async Task<ActionResult<List<CookBookDTO>>> GetAll()
    {
        var cookBooks = await _cookBookService.GetAll();

        if (cookBooks == null) return NotFound();

        return Ok(cookBooks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CookBookDTO>> Get(Guid id)
    {
        var cookBook = await _cookBookService.Get(id);
        
        if (cookBook == null) return NotFound();
        
        return Ok(cookBook);
    }

    [HttpPut]
    public async Task<ActionResult<CookBookDTO>> UpdateCookBook(CookBookUpdateDTO cookBookToUpdate)
    {
        var cookBook = await _cookBookService.Update(cookBookToUpdate);

        if (cookBook == null) return NotFound();

        return Ok(cookBook);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<CookBookDTO>> Delete(Guid id)
    {
        var cookBook = await _cookBookService.Delete(id);

        if (cookBook == null) return NotFound();

        return Ok(cookBook);
    }
}
