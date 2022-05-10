using System.Linq.Expressions;
using BlogWebAPI.Data.Models;
using BlogWebAPI.Models;

namespace BlogWebAPI.Data;

public interface IBlogRepository<T> where T: EntityModel
{
    //Create
    Task<Guid> Create(T entity);
    
    // Read
    Task<T?> GetById(Guid id);
    Task<T?> GetFirstWhere<TOrder>(
        Expression<Func<T, bool>> whereExp, 
        Expression<Func<T, TOrder>> orderByExp);

    Task<List<T>> GetAllWhere<TOrder>(
        Expression<Func<T, bool>> whereExp,
        Expression<Func<T, TOrder>> orderByExp,
        int limit = 1000);
    Task<PaginationResult<T>> GetAll(int page, int perPage);
    Task<PaginationResult<T>> GetAllWhere(int page, int perPage, Expression<Func<T, bool>> whereExp);
    
    // Update
    Task<T> Update(T entity);
    
    // Delete
    Task<bool> Delete(Guid id);
}