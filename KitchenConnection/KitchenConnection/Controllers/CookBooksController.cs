using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Models.Entities;
using KitchenConnection.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenConnection.Controllers;
    [ApiController]
    public class CookBooksController : ControllerBase
    {
        private readonly ICookBookService _cookBookService;
        public CookBooksController(ICookBookService cookBookService)
        {
            _cookBookService = cookBookService;
        }

        [HttpGet("GetAllCookBooks")]
        public async Task<IActionResult> GetCookBooks()
        {
            var CookBooks = await _cookBookService.GetCookBooks();
            return Ok(CookBooks);
        }
        [HttpGet("GetSingleCookBook")]
        public async Task<IActionResult> GetCookBook(Guid id)
        {
            var CookBook = await _cookBookService.GetCookBook(id);
            if (CookBook == null)
            {
                return NotFound();
            }
            return Ok(CookBook);
        }

        [HttpPost("CreateCookBook")]
        public async Task<IActionResult> CreateCookBook(CookBook cookBookToCreate)
        {
            await _cookBookService.CreateCookBook(cookBookToCreate);

            return Ok("CookBook created successfully!");
        }
        [HttpPut("UpdateCookBook")]
        public async Task<IActionResult> UpdateCookBook(CookBook cookbook)
        {
            await _cookBookService.UpdateCookBook(cookbook);
            return Ok("CookBook updated Successfully!");
        }
        [HttpDelete("DeleteCookBook")]
        public async Task<IActionResult> DeleteCookBook(Guid id)
        {
            await _cookBookService.DeleteCookBook(id);
            return Ok("CookBook Deleted Successfully!");
        }
    }
