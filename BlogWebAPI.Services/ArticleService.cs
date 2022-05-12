using BlogWebAPI.Models;
using BlogWebAPI.Services.Interfaces;
using BlogWebAPI.Services.Models;

namespace BlogWebAPI.Services;

public class ArticleService: IArticleService
{
    public Task<PagedServiceResult<ArticleDto>> GetAll(int page, int perPage)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult<ArticleDto>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult<ArticleDto>> Update(Guid id, ArticleDto article)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult<Guid>> Create(ArticleDto article)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult<Guid>> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}