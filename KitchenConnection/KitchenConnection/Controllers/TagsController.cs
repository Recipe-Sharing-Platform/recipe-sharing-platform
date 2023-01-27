using KitchenConnection.Application.Models.DTOs.Recipe;
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
            var tags = await _tagService.GetTags();

            if (tags != null)
            {
                return Ok(tags);
            }
            else
            {
                return NotFound("Could not be found!");
            }            
        }

        [HttpGet("GetSingleTag")]
        public async Task<IActionResult> GetTag(Guid id)
        {
            var tag = await _tagService.GetTag(id);
            if(tag == null)
            {
                return NotFound("Could not find tag!");
            }
            return Ok(tag);
        }

        [HttpPost("CreateTag")]
        public async Task<IActionResult> CreateTag(TagCreateDTO TagToCreate)
        {
            var createdTag=await _tagService.CreateTag(TagToCreate);

            if (createdTag != null)
            {
                return Ok(createdTag);
            }

            else
            {
                return BadRequest("Could not create tag!");
            }

        }

        //NOTE: Service needs to be reviewed
       /* [HttpPut("UpdateTag")]
        public async Task<IActionResult> UpdateTag(TagDTO tag) 
        {
            await _tagService.UpdateTag(tag);
            return Ok("Tag updated Successfully!");
        }*/

        [HttpDelete("DeleteTag")]
        public async Task<IActionResult> DeleteTag(Guid id)
        {
            var res=await _tagService.DeleteTag(id);

            if (res)
            {
                return Ok("Tag Deleted Successfully!");
            }
            else
            {
                return BadRequest("Could not delete tag!");
            }          
        }
    }
}
