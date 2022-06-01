using BlogWebAPI.Models;
using BlogWebAPI.Services.Interfaces;
using BlogWebAPI.Web.Models;
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

    /// <summary>
    /// Handles a GET request for retrieving a Tag.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("/tag/{id}")]
    public async Task<ActionResult> GetTag(string id)
    {
        try
        {
            var guid = Guid.Parse(id);
            var tag = await _tagService.GetById(guid);

            if (tag.Data == null && tag.Error == null)
            {
                _logger.LogWarning($"The requested Tag was not found: {id}");
                return NotFound($"Tag was not found: {id}");
            }

            if (tag.Error != null)
            {
                _logger.LogError($"Error retrieving tag: {id}, {tag.Error}");
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error retrieving the Tag");
            }
            
            _logger.LogDebug($"Retrieved a Tag: {id}");
            return Ok(tag.Data);
        }
        catch (FormatException e)
        {
            _logger.LogWarning($"There was a GUID format related error for the Tag: {id}"
            + e.Message + "\n" + e.StackTrace);
            return BadRequest(id);
        }
    }

    /// <summary>
    /// Handles a GET request for retrieving a paginated list of Tags.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("/tag")]
    public async Task<ActionResult> GetPaginatedTags([FromQuery] ManyTagsRequest query)
    {
        var page = query.Page == 0 ? 1 : query.Page;
        var perPage = query.PerPage == 0 ? 10 : query.PerPage;
        var tags = await _tagService.GetAll(page, perPage);

        if (tags.Error != null)
        {
            _logger.LogError($"Error retrieving paginated Tags: {tags.Error}");
            return StatusCode(StatusCodes.Status500InternalServerError, "There was an error retrieving the Tags");
        }
        
        _logger.LogDebug($"Retrieved tags total: {tags.Data.TotalCount}");
        return Ok(tags.Data);
    }

    /// <summary>
    /// Handles PUT requests for updating a Tag.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tagDto"></param>
    /// <returns></returns>
    public async Task<ActionResult> UpdateTag(string id, [FromBody] TagDto tagDto)
    {
        try
        {
            var guid = Guid.Parse(id);
            var updatedTag = await _tagService.Update(guid, tagDto);

            if (updatedTag.Error != null)
            {
                _logger.LogError($"Error updating the Tag {id}: {updatedTag.Error}");
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error updating the Tag.");
            }
            
            _logger.LogDebug($"Updated tag: {id}");
            return Ok(updatedTag.Data.ID);
        }
        catch (FormatException e)
        {
            _logger.LogWarning($"There was a GUID format related error for the Tag: {id}"
            + e.Message + "\n" + e.StackTrace);
            return BadRequest(id);
        }
    }

    /// <summary>
    /// Handles a DELETE request for deleting a Tag.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("/tag/{id}")]
    public async Task<ActionResult> DeleteTag(string id)
    {
        try
        {
            var guid = Guid.Parse(id);
            var deletedTag = await _tagService.Delete(guid);

            if (deletedTag.Error != null)
            {
                _logger.LogError($"Error deleting the Tag {id}: {deletedTag.Error}");
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error deleting the Tag.");
            }
            
            _logger.LogDebug($"Deleted the Tag: {id}");
            return Ok(new {id = deletedTag.Data});
        }
        catch (FormatException e)
        {
            _logger.LogWarning($"There was a GUID related format error for Tag {id}"
            + e.Message + "\n" + e.StackTrace);
            return BadRequest(id);
        }
    }
}