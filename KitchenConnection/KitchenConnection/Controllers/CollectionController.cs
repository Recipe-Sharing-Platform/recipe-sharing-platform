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

        public CollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        [HttpGet]  
        public async Task<List<CollectionDTO>> GetAll()
        {
        return await _collectionService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CollectionDTO>> Get(Guid id)
        {
            var collection = await _collectionService.Get(id);

            if (collection == null) return NotFound();

            return Ok(collection);
        }
        [HttpPost]
        public async Task<ActionResult<CollectionDTO>> Create(CollectionCreateRequestDTO collectionToCreate)
        {
            var UserId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var collection = await _collectionService.Create(collectionToCreate, UserId);

            if (collection == null) return NotFound();

            return Ok(collection);
        }

        [HttpPut]
        public async Task<ActionResult<CollectionDTO>> UpdateCollection(CollectionUpdateDTO collectionToUpdate)
        {
            var collection = await _collectionService.Update(collectionToUpdate);

            if (collection == null) return NotFound();

            return Ok(collection);
        }


    [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cookBook = await _collectionService.Delete(id);

            if (cookBook == null) return NotFound();

            return Ok(cookBook);
        }

}
