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
    
    
    
}