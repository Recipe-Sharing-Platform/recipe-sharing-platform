using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.Models.DTOs.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KitchenConnection.Controllers;

[ApiController]
[Route("api/tags")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;
    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TagDTO>>> GetAll()
    {
        var tags = await _tagService.GetAll();

        if(tags == null) return NotFound();
        
        return Ok(tags);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagDTO>> Get(Guid id)
    {
        var tag = await _tagService.Get(id);
        
        if(tag == null) return NotFound();

        return Ok(tag);
    }

    [HttpPost]
    public async Task<ActionResult<TagDTO>> Create(TagCreateDTO tagToCreate)
    {
        var tag = await _tagService.Create(tagToCreate);

        if (tag == null) return BadRequest("Tag could not be created!");
        
        return Ok(tag);
    }

    [HttpPut]
    public async Task<ActionResult<TagDTO>> Update(TagDTO tagToUpdate) 
    {
        var tag = await _tagService.Update(tagToUpdate);

        if (tag == null) return BadRequest("Tag could not be updated!");

        return Ok(tag);
    }

    [HttpDelete]
    public async Task<ActionResult<TagDTO>> Delete(Guid id)
    {
        var tag = await _tagService.Delete(id);

        if (tag == null) return BadRequest("Tag could not be deleted!");

        return Ok(tag);
    }
}