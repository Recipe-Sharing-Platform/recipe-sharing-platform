using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Models.Entities;
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
            var cookBooks = await _cookBookService.GetCookBooks();

            if (cookBooks != null)
            {
                return Ok(cookBooks);
            }
            else
            {
                return NotFound();
            }
            
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
            var createdCookBook=await _cookBookService.CreateCookBook(cookBookToCreate);

            if (createdCookBook != null)
            {
                return Ok(createdCookBook);
            }
            else
            {
            return BadRequest();
            }
            
        }
        [HttpPut("UpdateCookBook")]
        public async Task<IActionResult> UpdateCookBook(CookBook cookbook)
        {
            var updateCookBook = await _cookBookService.UpdateCookBook(cookbook);
            if (updateCookBook != null) 
            {
                return Ok(updateCookBook);
            }
            else
            {
                return BadRequest("Couldn't update!");
            }
            
        }
        [HttpDelete("DeleteCookBook")]
        public async Task<IActionResult> DeleteCookBook(Guid id)
        {
            var res = await _cookBookService.DeleteCookBook(id);

            if (res)
            {
                return Ok("CookBook Deleted Successfully!");
            }
            else
            {
                return BadRequest("CookBook could not be deleted!");
            }
        }
        
    }
