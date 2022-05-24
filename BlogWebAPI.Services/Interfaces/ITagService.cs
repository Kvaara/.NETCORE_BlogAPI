using BlogWebAPI.Models;
using BlogWebAPI.Services.Models;

namespace BlogWebAPI.Services.Interfaces;

public interface ITagService
{
    Task<PagedServiceResult<TagDto>> GetAll(int page, int perPage);
    Task<ServiceResult<TagDto>> GetById(Guid id);
    Task<ServiceResult<TagDto>> Update(Guid id, TagDto tagDto);
    Task<ServiceResult<Guid>> Create(TagDto tagDto);
    Task<ServiceResult<Guid>> Delete(Guid id);
}