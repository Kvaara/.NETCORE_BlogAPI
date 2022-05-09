using System.Linq.Expressions;
using BlogWebAPI.Data.Models;
using BlogWebAPI.Models;

namespace BlogWebAPI.Data;

public interface IBlogRepository<T> where T: EntityModel
{
    //Create
    Task<Guid> Create(T entity);
    
    // Read
    Task<T> GetById(Guid id);
    Task<T> GetFirstWhere<TOrder>(Expression<Func<T, bool>> whereExp, Expression<Func<T, TOrder>> orderByExp);
    Task<PaginationResult<T>> GetAll(int page, int perPage);
    Task<PaginationResult<T>> GetAll(int page, int perPage, Expression<Func<T, bool>> orderByExp);
    
    // Update
    Task<T> Update(T entity);
    
    // Delete
    Task<bool> Delete(Guid id);
}