using AutoMapper;
using BlogWebAPI.Data;
using BlogWebAPI.Data.Models;
using BlogWebAPI.Models;
using BlogWebAPI.Services.Interfaces;
using BlogWebAPI.Services.Models;
using Microsoft.Extensions.Logging;

namespace BlogWebAPI.Services;

public class ArticleService: IArticleService
{
    private readonly IBlogRepository<Article> _articlesRepo;
    private readonly IBlogRepository<ArticleTag> _articleTags;
    private readonly IBlogRepository<Tag> _tags;
    private readonly IBlogRepository<User> _users;
    private readonly ILogger<ArticleService> _logger;
    private readonly IMapper _mapper;
    public ArticleService(
        IBlogRepository<Article> articlesRepo,
        IBlogRepository<ArticleTag> articleTags,
        IBlogRepository<Tag> tags,
        IBlogRepository<User> users,
        ILogger<ArticleService> logger,
        IMapper mapper
    )
    {
        _articlesRepo = articlesRepo;
        _articleTags = articleTags;
        _tags = tags;
        _users = users;
        _logger = logger;
        _mapper = mapper;
    }
    
    public async Task<PagedServiceResult<ArticleDto>> GetAll(int page, int perPage)
    {
        var articles = await _articlesRepo.GetAll(page, perPage);

        var articleModels = new List<ArticleDto>();
        
        // Adding the corresponding tags and users to the Article data transfer objects
        foreach (var article in articles.Results)
        {
            var articleModel = _mapper.Map<ArticleDto>(article);
            
            // Getting all of the associated Article tags:
            var articleTags = await _articleTags.GetAllWhere();
        }
        
        var articlesResultModel = new PaginationResult<ArticleDto>
        {
            TotalCount = 0,
            Results = null,
            ResultsPerPage = 0,
            PageNumber = null,
        }
        
        return new PagedServiceResult<ArticleDto>
        {
            IsSuccess = true,
            Data = articlesResultModel,
            Error = null,
        }
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