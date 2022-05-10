using System.Linq.Expressions;
using BlogWebAPI.Data.Models;
using BlogWebAPI.Models;

namespace BlogWebAPI.Data;

public class BlogRepository<T>: IBlogRepository<T> where T: EntityModel
{
    public Task<Guid> Create(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetFirstWhere<TOrder>(Expression<Func<T, bool>> whereExp, Expression<Func<T, TOrder>> orderByExp)
    {
        throw new NotImplementedException();
    }

    public Task<List<T>> GetAllWhere<TOrder>(Expression<Func<T, bool>> whereExp, Expression<Func<T, TOrder>> orderByExp, int limit = 1000)
    {
        throw new NotImplementedException();
    }

    public Task<PaginationResult<T>> GetAll(int page, int perPage)
    {
        throw new NotImplementedException();
    }

    public Task<PaginationResult<T>> GetAll(int page, int perPage, Expression<Func<T, bool>> orderByExp)
    {
        throw new NotImplementedException();
    }

    public Task<T> Update(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}