using Microsoft.AspNetCore.Mvc;

namespace BlogWebAPI.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticleController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    /*private readonly ILogger<ArticleController> _logger;

    public ArticleController(ILogger<ArticleController> logger)
    {
        _logger = logger;
    }*/

    [HttpGet("/article")]
    public ActionResult GetPaginatedArticles()
    {
        return Ok("Hello!");
    }
}
