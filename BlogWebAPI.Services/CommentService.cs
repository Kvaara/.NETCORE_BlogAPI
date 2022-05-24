using AutoMapper;
using BlogWebAPI.Data;
using BlogWebAPI.Data.Models;
using BlogWebAPI.Models;
using BlogWebAPI.Services.Models;

namespace BlogWebAPI.Services;

/// <summary>
/// Handles service-level logic related to Blog Comments.
/// </summary>
public class CommentService
{
    private readonly IBlogRepository<Comment> _comments;
    private readonly IMapper _mapper;

    public CommentService(IBlogRepository<Comment> repo, IMapper mapper)
    {
        _comments = repo;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets paginated comments.
    /// </summary>
    /// <param name="page">Current Page Number</param>
    /// <param name="perPage">Number of comments to show per page.</param>
    /// <param name="articleId">Option article ID for filter</param>
    /// <returns></returns>
    public async Task<PagedServiceResult<CommentDto>> GetAll(int page, int perPage, Guid? articleId = null)
    {
        try
        {
            PaginationResult<Comment> comments;

            // If an Article ID is provided, use the GetAll override allowing an expression
            // to get the comments just for this article.
            
            if (articleId != null)
            {
                comments = await _comments.GetAllWhere(page, perPage, cmt => cmt.ArticleID == articleId);
            }

            else
            {
                comments = await _comments.GetAll(page, perPage);
            }

            var commentsModel = new PaginationResult<CommentDto>
            {
                PageNumber = comments.PageNumber,
                ResultsPerPage = comments.ResultsPerPage,
                TotalCount = comments.TotalCount,
                Results = _mapper.Map<List<CommentDto>>(comments.Results),
            };

            return new PagedServiceResult<CommentDto>
            {
                IsSuccess = true,
                Data = commentsModel,
                Error = null,
            };
        }
        catch (Exception e)
        {
            return new PagedServiceResult<CommentDto>
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

    /// <summary>
    /// Gets an Comment.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ServiceResult<CommentDto>> GetById(Guid id)
    {
        try
        {
            var comment = await _comments.GetById(id);

            return new ServiceResult<CommentDto>
            {
                IsSuccess = true,
                Data = _mapper.Map<CommentDto>(comment),
                Error = null,
            };

        }
        catch (Exception e)
        {
            return new ServiceResult<CommentDto>
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

    /// <summary>
    /// Creates an Comment.
    /// </summary>
    /// <param name="commentDto"></param>
    /// <returns></returns>
    public async Task<ServiceResult<Guid>> Create(CommentDto commentDto)
    {
        try
        {
            var comment = _mapper.Map<Comment>(commentDto);

            var newCommentId = await _comments.Create(comment);

            return new ServiceResult<Guid>
            {
                IsSuccess = true,
                Data = newCommentId,
                Error = null
            };
        }
        catch (Exception e)
        {
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

    /// <summary>
    /// Updates an Comment.
    /// </summary>
    /// <param name="commentId"></param>
    /// <param name="commentDto"></param>
    /// <returns></returns>
    public async Task<ServiceResult<CommentDto>> Update(Guid commentId, CommentDto commentDto)
    {
        try
        {
            var commentToUpdate = await _comments.GetById(commentId);

            commentToUpdate.Message = commentDto.Message;
            commentToUpdate.CommenterName = commentDto.CommenterName;

            var updatedComment = await _comments.Update(commentToUpdate);

            var commentResult = _mapper.Map<CommentDto>(updatedComment);

            return new ServiceResult<CommentDto>
            {
                IsSuccess = true,
                Data = commentResult,
                Error = null,
            };
        }
        catch (Exception e)
        {
            return new ServiceResult<CommentDto>
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

    /// <summary>
    /// Deletes a Comment.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ServiceResult<Guid>> Delete(Guid id)
    {
        try
        {
            await _comments.Delete(id);

            return new ServiceResult<Guid>
            {
                IsSuccess = true,
                Data = id,
                Error = null,
            };
        }
        catch (Exception e)
        {
            return new ServiceResult<Guid>
            {
                IsSuccess = false,
                Data = id,
                Error = new ServiceError
                {
                    Stacktrace = e.StackTrace,
                    Message = e.Message,
                }
            };
        }
    }
}