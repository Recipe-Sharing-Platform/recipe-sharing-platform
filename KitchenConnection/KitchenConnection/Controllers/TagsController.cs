using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.DataLayer.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenConnection.Controllers
{
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;
        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet("GetAllTags")]
        public async Task<IActionResult> GetTags()
        {
            var Tags = await _tagService.GetTags();
            return Ok(Tags);
        }
        [HttpGet("GetSingleTag")]
        public async Task<IActionResult> GetTag(string id)
        {
            var Tag = await _tagService.GetTag(id);
            if(Tag == null)
            {
                return NotFound();
            }
            return Ok(Tag);
        }

        [HttpPost("CreateTag")]
        public async Task<IActionResult> CreateTag(Tag TagToCreate)
        {
            await _tagService.CreateTag(TagToCreate);

            return Ok("Tag created successfully!");
        }
        [HttpPut("UpdateTag")]
        public async Task<IActionResult> UpdateTag(Tag tag) 
        {
            await _tagService.UpdateTag(tag);
            return Ok("Tag updated Successfully!");
        }
        [HttpDelete("DeleteTag")]
        public async Task<IActionResult> DeleteTag(string id)
        {
            await _tagService.DeleteTag(id);
            return Ok("Tag Deleted Successfully!");
        }
    }
}
