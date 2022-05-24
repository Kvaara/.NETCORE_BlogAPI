using BlogWebAPI.Models;
using BlogWebAPI.Services.Models;

namespace BlogWebAPI.Services.Interfaces;

public interface IUserService
{
    Task<PagedServiceResult<UserDto>> GetAll(int page, int perPage);
    Task<ServiceResult<UserDto>> GetById(Guid id);
    Task<ServiceResult<UserDto>> Update(Guid id, UserDto userDto);
    Task<ServiceResult<Guid>> Create(UserDto userDto);
    Task<ServiceResult<Guid>> Delete(Guid id);
}