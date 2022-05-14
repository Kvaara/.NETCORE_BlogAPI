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
    private readonly IBlogRepository<ArticleTag> _articleTagsRepo;
    private readonly IBlogRepository<Tag> _tagsRepo;
    private readonly IBlogRepository<User> _usersRepo;
    private readonly ILogger<ArticleService> _logger;
    private readonly IMapper _mapper;
    public ArticleService(
        IBlogRepository<Article> articlesRepo,
        IBlogRepository<ArticleTag> articleTagsRepo,
        IBlogRepository<Tag> tagsRepo,
        IBlogRepository<User> usersRepo,
        ILogger<ArticleService> logger,
        IMapper mapper
    )
    {
        _articlesRepo = articlesRepo;
        _articleTagsRepo = articleTagsRepo;
        _tagsRepo = tagsRepo;
        _usersRepo = usersRepo;
        _logger = logger;
        _mapper = mapper;
    }
    
    public async Task<PagedServiceResult<ArticleDto>> GetAll(int page, int perPage)
    {
        try
        {
            var articles = await _articlesRepo.GetAll(page, perPage);

            var articleModels = new List<ArticleDto>();
        
            // Adding the corresponding tags and users to the Article data transfer objects
            foreach (var article in articles.Results)
            {
                var articleModel = _mapper.Map<ArticleDto>(article);
            
                // Getting all of the associated Article tags (join table):
                var articleTags = await _articleTagsRepo.GetAllWhere(
                    at => at.ArticleID == article.ID,
                    at => at.CreatedOn);
            
                // Getting the associated Tag Ids:
                var tagIds = articleTags.Select(t => t.TagID).ToList();
            
                // Getting the Tag entity instances:
                var tags = await _tagsRepo.GetAllWhere(
                    tag => tagIds.Contains(tag.ID),
                    tag => tag.CreatedOn);

                // Map/Assign all of the tag names into the AutoMapped articleModel.
                if (articleTags.Count > 0)
                {
                    articleModel.Tags = tags.Select(tag => tag.Name).ToList();
                }

                var author = await _usersRepo.GetFirstWhere(
                    user => user.ID == article.AuthorID,
                    user => user.UpdatedOn);

                articleModel.AuthorName = author.UserName;
            
                // Adding the build AutoMapped articleModel into the high-level articleModels list.
                articleModels.Add(articleModel);
            }

            var articlesResultModel = new PaginationResult<ArticleDto>
            {
                TotalCount = articles.TotalCount,
                Results = articleModels,
                ResultsPerPage = articles.ResultsPerPage,
                PageNumber = articles.PageNumber,
            };

            _logger.LogDebug($"Returning paginated articles. Page {page}, perPage {perPage}");
        
            return new PagedServiceResult<ArticleDto>
            {
                IsSuccess = true,
                Data = articlesResultModel,
                Error = null,
            };
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed to return paginated articles. {e.StackTrace}");

            return new PagedServiceResult<ArticleDto>
            {
                IsSuccess = false,
                Data = null,
                Error = new ServiceError
                {
                    Stacktrace = e.StackTrace,
                    Message = e.Message,
                }
            };
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