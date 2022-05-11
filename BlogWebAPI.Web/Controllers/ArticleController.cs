using BlogWebAPI.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebAPI.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticleController : ControllerBase
{
    private readonly ILogger<ArticleController> _logger;
    private readonly IArticleService _articleService;
    public ArticleController(
        ILogger<ArticleController> logger,
        IArticleService articleService
        )
    {
        _logger = logger;
        _articleService = articleService;
    }

    [HttpGet("/article")]
    public async Task<ActionResult> GetPaginatedArticles([FromQuery] ManyArticlesRequest query)
    {
        // 1 and 3 are so called "magic numbers".
        // They should be written more cleanly and transferred to static variables that are more understandable.
        var page = query.Page == 0 ? 1 : query.Page;
        var perPage = query.PerPage == 0 ? 3 : query.PerPage;
        var articles = await _articleService.GetAll(page, perPage);
        // return Ok("Hello!");
    }
}
