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

    /// <summary>
    /// Handles a POST request for creating an Article
    /// </summary>
    /// <param name="article"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Handles a GET request that returns a possible Article based on a given id. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("/article/{id}")]
    public async Task<ActionResult> GetArticle(string id)
    {
        try
        {
            var guid = Guid.Parse(id);
            var article = await _articleService.GetById(guid);

            if (article.Data is null && article.Error is null)
            {
                _logger.LogWarning($"Requested Article not found: {id}");
                return NotFound($"Article not found: {id}");
            }

            if (article.Error is not null)
            {
                _logger.LogError($"Error retrieving an Article: {id}, {article.Error}");
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error retrieving an Article.");
            }

            _logger.LogDebug($"Retrieved an Article: {id}");
            return Ok(article.Data);
        }
        catch (FormatException err)
        {
            _logger.LogWarning($"There was a GUID related formatting error for article: {id}." 
                               + err.Message 
                               + "\n"
                               + err.StackTrace);
            return BadRequest(id);
        }
    }

    /// <summary>
    /// Handles a GET request for getting a paginated lists of Articles.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
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

    [HttpPut("/article/{id}")]
    public async Task<ActionResult> UpdateArticle(string id, [FromBody] ArticleDto article)
    {
        try
        {
            var guid = Guid.Parse(id);
            var updatedArticle = await _articleService.Update(guid, article);

            if (updatedArticle.Error is not null)
            {
                _logger.LogError($"Error updating the Article {id}: {updatedArticle.Error}");
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error updating the Article");
            } 

            _logger.LogDebug($"Updated the Article: {id}");
            return Ok(updatedArticle.Data.ID);
        }
        catch (Exception e)
        {
            _logger.LogWarning($"There was a GUID format error for an Article: {id}"
            + e.Message + "\n" + e.StackTrace);
            return BadRequest(id);
        }
    }

    [HttpDelete("/article/{id}")]
    public async Task<ActionResult> DeleteArticle(string id)
    {
        try
        {
            var guid = Guid.Parse(id);
            var deletedArticle = await _articleService.Delete(guid);

            if (deletedArticle.Error is not null)
            {
                _logger.LogError($"Error deleting the Article {id}: {deletedArticle.Error}");
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error deleting the Article");
            } 

            _logger.LogDebug($"Deleted the Article: {id}");
            return Ok(deletedArticle.Data.ID);
        }
        catch (Exception e)
        {
            _logger.LogWarning($"There was a GUID format error for Deleting the Article: {id}"
                               + e.Message + "\n" + e.StackTrace);
            return BadRequest(id);
        }
    }
}
