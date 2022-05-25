using BlogWebAPI.Models;
using BlogWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebAPI.Web.Controllers;

/// <summary>
/// Handles HTTP-requests related to Comments.
/// </summary>
[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly ICommentService _commentService;

    public CommentController(
        ILogger<CommentController> logger,
        ICommentService commentService)
    {
        _logger = logger;
        _commentService = commentService;
    }

    /// <summary>
    /// Handles a POST-requests for creating a comment.
    /// </summary>
    /// <param name="commentDto"></param>
    /// <returns></returns>
    [HttpPost("/comment")]
    public async Task<ActionResult> CreateComment([FromBody] CommentDto commentDto)
    {
        _logger.LogDebug($"Creating new Comment for an Article with ID: {commentDto.ArticleID}");

        var newComment = await _commentService.Create(commentDto);

        if (newComment.Error != null)
        {
            _logger.LogError($"Service level error creating a Comment: {newComment.Error}");
            return StatusCode(StatusCodes.Status500InternalServerError, "There was an error adding a new Comment.");
        }
        
        _logger.LogError($"Added a new Comment: {newComment.Data}");
        return StatusCode(StatusCodes.Status201Created, new {id=newComment.Data});
    }

    /// <summary>
    /// Handles GET-requests for retrieving a Comment.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("/comment/{id}")]
    public async Task<ActionResult> GetComment(string id)
    {
        try
        {
            var guid = Guid.Parse(id);
            var comment = await _commentService.GetById(guid);

            if (comment.Data == null && comment.Error == null)
            {
                _logger.LogWarning($"Comment not found: {id}");
                return NotFound($"Comment not found: {id}");
            }

            if (comment.Error != null)
            {
                _logger.LogWarning($"Error retrieving the Comment: {id} {comment.Error}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving the Comment.");
            }
            
            _logger.LogDebug($"Retrieved a Comment: {id}");
            return Ok(comment.Data)
        }
        catch (Exception e)
        {
            _logger.LogWarning($"There was a GUID related format error for a Comment: {id}" + e.Message + "\n" + e.StackTrace);
            return BadRequest(id);
        }
    }
}