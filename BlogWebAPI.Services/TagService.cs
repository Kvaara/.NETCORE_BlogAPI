using BlogWebAPI.Data.Models;
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
    /// Gets a Tag.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ServiceResult<TagDto>> GetById(Guid id)
    {
        try
        {
            var tag = await _tags.GetById(id);

            return new ServiceResult<TagDto>
            {
                IsSuccess = true,
                Data = _mapper.Map<TagDto>(tag),
                Error = null,
            };

        }
        catch (Exception e)
        {
            return new ServiceResult<TagDto>
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
    /// Creates a Tag.
    /// </summary>
    /// <param name="tagDto"></param>
    /// <returns></returns>
    public async Task<ServiceResult<Guid>> Create(TagDto tagDto)
    {
        try
        {
            var existingTag = await _tags.GetFirstWhere(t => t.Name == tagDto.Name, t => t.CreatedOn);

            if (existingTag != null)
            {
                // The Tag already exists, which is why we can send a custom error.
                return new ServiceResult<Guid>
                {
                    IsSuccess = false,
                    Error = new ServiceError()
                    {
                        Message = "TAG_EXISTS"
                    }
                };
            }

            var tag = _mapper.Map<Tag>(tagDto);
            var newTagID = await _tags.Create(tag);

            return new ServiceResult<Guid>
            {
                IsSuccess = true,
                Data = newTagID,
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