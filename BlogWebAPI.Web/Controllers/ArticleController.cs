using BlogWebAPI.Models;
using BlogWebAPI.Services.Interfaces;
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

    [HttpPost("/article")]
    public async Task<ActionResult> CreateArticle([FromBody] ArticleDto article)
    {
        _logger.LogDebug($"Creating a new article: {article.Title}");
        var newArticle = await _articleService.Create(article);

        if (newArticle.Error is not null)
        {
            _logger.LogError($"Service level error creating  an Article: {newArticle.Error}");
            return StatusCode(StatusCodes.Status500InternalServerError, "There was an error creating the Article.");
        }
        
        _logger.LogDebug($"Created an Article: {article.Title} ({newArticle.Data})");
        return StatusCode(StatusCodes.Status201Created, new {id = newArticle.Data});
    }

    [HttpGet("/article/{id}")]
    public async Task<ActionResult> GetArticle(string id)
    {
        try
        {
        var guid = Guid.Parse(id);
        var article = await _articleService.GetById(guid);
        }
        catch (FormatException err)
        {
            _logger.LogWarning($"There was a GUID related formatting error for article: {id}");
            return BadRequest(id);
        }
    }

    [HttpGet("/article")]
    public async Task<ActionResult> GetPaginatedArticles([FromQuery] ManyArticlesRequest query)
    {
        // 1 and 3 are so called "magic numbers".
        // They should be written more cleanly and transferred to static variables that are more understandable/readable.
        var page = query.Page == 0 ? 1 : query.Page;
        var perPage = query.PerPage == 0 ? 3 : query.PerPage;
        var articles = await _articleService.GetAll(page, perPage);

        if (articles.Error is not null)
        {
            _logger.LogError($"Error retrieving paginated articles: {articles.Error}");
            return StatusCode(StatusCodes.Status500InternalServerError, "There was an error retrieving articles");
        }
        
        _logger.LogDebug($"Retrieved Articles total: {articles.Data.TotalCount}");
        return Ok(articles.Data);
    }
}
