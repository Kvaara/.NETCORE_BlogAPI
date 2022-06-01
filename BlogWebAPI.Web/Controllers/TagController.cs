using BlogWebAPI.Models;
using BlogWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebAPI.Web.Controllers;

/// <summary>
/// Handles HTTP requests related to Tags.
/// </summary>
[ApiController]
[Route("[controller]")]
public class TagController : ControllerBase
{
    private readonly ILogger<TagController> _logger;
    private readonly ITagService _tagService;

    public TagController(
        ILogger<TagController> logger,
        ITagService tagService)
    {
        _logger = logger;
        _tagService = tagService;
    }

    /// <summary>
    /// Handles a POST request for creating a Tag.
    /// </summary>
    /// <param name="tagDto"></param>
    /// <returns></returns>
    [HttpPost("/tag")]
    public async Task<ActionResult> CreateTag([FromBody] TagDto tagDto)
    {
        _logger.LogDebug($"Creating a new Tag: {tagDto.Name}");
        var newTag = await _tagService.Create(tagDto);

        if (newTag.Error != null && newTag.Error.Message == "TAG_EXISTS")
        {
            _logger.LogWarning($"Service level warning: attempted tto create a duplicate Tag: {newTag.Error}");
            return StatusCode(StatusCodes.Status409Conflict, $"The Tag {tagDto.Name} already exists.");
        }

        if (newTag.Error != null)
        {
            _logger.LogError($"Service level error creating a Tag: {newTag.Error}");
            return StatusCode(StatusCodes.Status500InternalServerError, "There was an error creating a Tag");
        }
        
        _logger.LogDebug($"Created a Tag: {tagDto.Name} ({newTag.Data})");
        return StatusCode(StatusCodes.Status201Created, new {id = newTag.Data});
    }
}