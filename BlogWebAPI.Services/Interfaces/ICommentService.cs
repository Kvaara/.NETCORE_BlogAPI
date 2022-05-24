using BlogWebAPI.Models;
using BlogWebAPI.Services.Models;

namespace BlogWebAPI.Services.Interfaces;

public interface ICommentService
{
    Task<PagedServiceResult<CommentDto>> GetAll(int page, int perPage, Guid? articleId = null);
    Task<ServiceResult<CommentDto>> GetById(Guid id);
    Task<ServiceResult<CommentDto>> Update(Guid id, CommentDto commentDto);
    Task<ServiceResult<Guid>> Create(CommentDto commentDto);
    Task<ServiceResult<Guid>> Delete(Guid id);
}