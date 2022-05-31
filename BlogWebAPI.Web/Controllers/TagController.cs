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
}