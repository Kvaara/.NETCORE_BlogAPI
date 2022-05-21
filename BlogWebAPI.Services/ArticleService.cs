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

    public async Task<ServiceResult<ArticleDto>> GetById(Guid id)
    {
        try
        {
            var article = await _articlesRepo.GetById(id);
            if (article == null)
            {
                return new ServiceResult<ArticleDto>
                {
                    IsSuccess = true,
                    Data = null,
                    Error = null,
                };
            }

            var articleModel = _mapper.Map<ArticleDto>(article);

            var author = await _usersRepo.GetFirstWhere(
                user => user.ID == article.AuthorID,
                user => user.UpdatedOn);

            articleModel.AuthorName = author.UserName;

            articleModel.Tags = new List<string>();

            var articleTags = await _articleTagsRepo.GetAllWhere(
                tag => tag.ArticleID == id,
                tag => tag.CreatedOn);

            if (articleTags.Count > 0)
            {
                var tagIds = articleTags.Select(at => at.TagID).ToList();
                var tags = await _tagsRepo.GetAllWhere(
                    tag => tagIds.Contains(tag.ID),
                    tag => tag.CreatedOn);
                articleModel.Tags = tags.Select(tag => tag.Name).ToList();
            }
        
            _logger.LogDebug($"Returning an Article with ID: {id}");
            return new ServiceResult<ArticleDto>
            {
                IsSuccess = true,
                Data = articleModel,
                Error = null,
            };
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed to fetch an Article with id: {id}.");

            return new ServiceResult<ArticleDto>
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

    public async Task<ServiceResult<ArticleDto>> Update(Guid articleId, ArticleDto articleDto)
    {
        try
        {
            // Get the Article entity
            var articleToUpdate = await _articlesRepo.GetById(articleId);

            // Update the Article entity
            articleToUpdate.Title = articleDto.Title;
            articleToUpdate.Content = articleDto.Content;
            
            //  Delete all the ArticleTags on the existing entity
            if (articleToUpdate.ArticleTags != null)
            {
                _logger.LogDebug($"Deleting existing ArticleTags for an Article with ID: {articleId}");
                foreach (var articleTagEntity in articleToUpdate.ArticleTags)
                {
                    await _articleTagsRepo.Delete(articleTagEntity.ID);
                }
            }
            
            // If there are tags on the update request object, create the ArticleTags anew
            if (articleDto.Tags != null && articleDto.Tags.Count > 0)
            {
                _logger.LogDebug($"Creating updated ArticleTags for an Article with ID: {articleId}");
                foreach (var newTag in articleDto.Tags)
                {
                    var newTagGuid = Guid.Parse(newTag);
                    var tag = await _tagsRepo.GetById(newTagGuid);

                    var newArticleTag = new ArticleTag
                    {
                        ID = Guid.NewGuid(),
                        Article = articleToUpdate,
                        Tag = tag
                    };
                    await _articleTagsRepo.Create(newArticleTag);
                }
            }
            
            // Save the Article entity
            var updatedArticle = await _articlesRepo.Update(articleToUpdate);

            // Map back to a view model => return result
            var articleResult = _mapper.Map<ArticleDto>(updatedArticle);
            
            _logger.LogDebug($"Updated an Article with ID: {articleId}");
            return new ServiceResult<ArticleDto>
            {
                IsSuccess = true,
                Data = articleResult,
                Error = null,
            };
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed to update an Article: {e}.");

            return new ServiceResult<ArticleDto>
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

    public async Task<ServiceResult<Guid>> Create(ArticleDto articleDto)
    {
        try
        {
            var article = _mapper.Map<Article>(articleDto);
            var authorGuid = Guid.Parse(articleDto.AuthorId);
            var author = await _usersRepo.GetById(authorGuid);
            article.Author = author;
            
            var newArticleId = await _articlesRepo.Create(article);

            if (articleDto.Tags == null)
            {
                _logger.LogDebug($"Returning a new Article with id: {newArticleId}.");
                
                return new ServiceResult<Guid>
                {
                    IsSuccess = true,
                    Data = newArticleId,
                    Error = null,
                };
            }

            foreach (var articleTag in articleDto.Tags)
            {
                var articleTagGuid = Guid.Parse(articleTag);
                var foundArticleTag = await _tagsRepo.GetFirstWhere(
                    tag => tag.ID == articleTagGuid,
                    tag => tag.CreatedOn);
                
                if (foundArticleTag == null) continue;

                var newArticleTag = new ArticleTag
                {
                    ID = Guid.NewGuid(),
                    Article = article,
                    Tag = foundArticleTag,
                };

                await _articleTagsRepo.Create(newArticleTag);
            }
            _logger.LogDebug($"Returning a new Article with id: {newArticleId}.");

            return new ServiceResult<Guid>
            {
                IsSuccess = true,
                Data = newArticleId,
                Error = null,
            };
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed to create an Article: {e}.");

            return new ServiceResult<Guid>
            {
                IsSuccess = false,
                Data = Guid.Empty,
                Error = new ServiceError
                {
                    Stacktrace = e.StackTrace,
                    Message = e.Message,
                }
            };
        }
    }

    public async Task<ServiceResult<Guid>> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}