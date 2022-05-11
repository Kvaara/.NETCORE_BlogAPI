using BlogWebAPI.Services.Models;

namespace BlogWebAPI.Services.Interfaces;

public interface IArticleService
{
    Task<PagedServiceResult<ArticleDto>> GetAll(int page, int perPage);
}