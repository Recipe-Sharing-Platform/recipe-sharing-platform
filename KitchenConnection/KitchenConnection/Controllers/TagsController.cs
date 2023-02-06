using KitchenConnection.BusinessLogic.Helpers.Exceptions.TagExceptions;
using KitchenConnection.BusinessLogic.Services.IServices;
using KitchenConnection.Models.DTOs.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KitchenConnection.Controllers;

[ApiController]
[Route("api/tags")]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;
    private readonly ILogger<TagsController> _logger;

    public TagsController(ITagService tagService, ILogger<TagsController> logger)
    {
        _tagService = tagService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<TagDTO>>> GetAll()
    {
        try
        {
            var tags = await _tagService.GetAll();

            return Ok(tags);
        }
        catch (TagsNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(TagsController)}, Method: {nameof(GetAll)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagDTO>> Get(Guid id)
    {
        try
        {
            var tag = await _tagService.Get(id);

            return Ok(tag);
        }
        catch (TagsNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(TagsController)}, Method: {nameof(Get)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<TagDTO>> Create(TagCreateDTO tagToCreate)
    {
        try
        {
            var tag = await _tagService.Create(tagToCreate);

            return Ok(tag);
        }
        catch (TagCouldNotBeCreatedException ex)
        {
            _logger.LogError($"Error at Class: {nameof(TagsController)}, Method: {nameof(Create)}, Exception: {ex}");
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "RSP_Admin")]
    public async Task<ActionResult<TagDTO>> Update(TagDTO tagToUpdate)
    {
        try
        {
            var tag = await _tagService.Update(tagToUpdate);

            return Ok(tag);
        }
        catch (TagNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(TagsController)}, Method: {nameof(Update)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "RSP_Admin")]
    public async Task<ActionResult<TagDTO>> Delete(Guid id)
    {
        try
        {
            var tag = await _tagService.Delete(id);

            return Ok(tag);
        }
        catch (TagNotFoundException ex)
        {
            _logger.LogError($"Error at Class: {nameof(TagsController)}, Method: {nameof(Delete)}, Exception: {ex}");
            return NotFound(ex.Message);
        }
    }
}