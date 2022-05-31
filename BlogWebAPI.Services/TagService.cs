using BlogWebAPI.Models;
using BlogWebAPI.Services.Interfaces;
using BlogWebAPI.Services.Models;

namespace BlogWebAPI.Services;

/// <summary>
/// Handles service-level logic related to Blog Article Tags.
/// </summary>
public class TagService : ITagService
{
    private readonly IBlogRepository<Tag> _tags;
    private readonly IMapper _mapper;

    public TagService(IBlogRepository<Tag> repo, IMapper mapper)
    {
        _tags = repo;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets paginated Tags.
    /// </summary>
    /// <param name="page">Current Page Number</param>
    /// <param name="perPage">Number of comments to show per page.</param>
    /// <returns></returns>
    public async Task<PagedServiceResult<TagDto>> GetAll(int page, int perPage)
    {
        try
        {
            var tags = await _tags.getAll(page, perPage);

            var tagsResultModel = new PaginationResult<TagDto>
            {
                PageNumber = tags.pageNumber,
                ResultsPerPage = tags.ResultsPerPage,
                TotalCount = tags.TotalCount,
                Results = _mapper.Map<List<TagDto>>(tags.Results),
            };

            return new PagedServiceResult<TagDto>
            {
                IsSuccess = true,
                Data = tagsResultModel,
                Error = null,
            };
        }
        catch (Exception e)
        {
            return new PagedServiceResult<TagDto>
            {
                IsSuccess = false,
                Data = null,
                Error = new ServiceError
                {
                    Stacktrace = e.StackTrace,
                    Message = e.Message
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