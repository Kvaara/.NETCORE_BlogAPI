using System.Linq.Expressions;
using BlogWebAPI.Data.Models;
using BlogWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogWebAPI.Data;

public class BlogRepository<T>: IBlogRepository<T> where T: EntityModel
{
    private readonly DbSet<T> _entities;
    private readonly BlogDbContext _db;
    
    public BlogRepository(BlogDbContext db)
    {
        _entities = db.Set<T>();
        _db = db;
    }
    
    /// <summary>
    /// Creates a resource in the database
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<Guid> Create(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        
        entity.CreatedOn = DateTime.UtcNow;
        entity.UpdatedOn = DateTime.UtcNow;
        _entities.Add(entity);
        await _db.SaveChangesAsync();
        return entity.ID;
    }

    /// <summary>
    /// Gets the entity with the provided Primary Key id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<T?> GetById(Guid id)
    {
        return await _entities.SingleOrDefaultAsync(ent => ent.ID == id);
    }

    /// <summary>
    /// Gets a first matching entity given the WHERE expression
    /// and the ORDER BY Expression
    /// </summary>
    /// <param name="whereExp"></param>
    /// <param name="orderByExp"></param>
    /// <typeparam name="TOrder"></typeparam>
    /// <returns></returns>
    public async Task<T?> GetFirstWhere<TOrder>(Expression<Func<T, bool>> whereExp, Expression<Func<T, TOrder>> orderByExp)
    {
        return await _entities
            .Where(whereExp)
            .OrderByDescending(orderByExp)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Gets a list of entities in the database
    /// given a whereExp (WHERE) and an orderBy (ORDER BY) Expression
    /// </summary>
    /// <param name="whereExp"></param>
    /// <param name="orderByExp"></param>
    /// <param name="limit"></param>
    /// <typeparam name="TOrder"></typeparam>
    /// <returns></returns>
    public async Task<List<T>> GetAllWhere<TOrder>(Expression<Func<T, bool>> whereExp, Expression<Func<T, TOrder>> orderByExp, int limit = 1000)
    {
        var entities = _entities
            .AsQueryable()
            .Where(whereExp);
        
        return await entities
            .Take(limit)
            .OrderByDescending(orderByExp)
            .ToListAsync();
    }

    /// <summary>
    /// Gets a Paginated collection of all entities inside the database
    /// Ordered by UpdatedOn DESC
    /// </summary>
    /// <param name="page"></param>
    /// <param name="perPage"></param>
    /// <returns></returns>
    public async Task<PaginationResult<T>> GetAll(int page = 1, int perPage = 3)
    {
        var count = await _entities.CountAsync();
        var entitiesToSkip = (page - 1) * perPage;
        var entities = await _entities
            .OrderByDescending(ent => ent.UpdatedOn)
            .Skip(entitiesToSkip)
            .Take(perPage)
            .ToListAsync();

        return new PaginationResult<T>
        {
            TotalCount = count,
            Results = entities,
            ResultsPerPage = perPage,
            PageNumber = page
        };
    }

    /// <summary>
    /// Gets a Paginated collection of all entities inside the database given the whereExp
    /// Ordered by UpdatedOn DESC
    /// </summary>
    /// <param name="page"></param>
    /// <param name="perPage"></param>
    /// <param name="whereExp"></param>
    /// <returns></returns>
    public async Task<PaginationResult<T>> GetAllWhere(int page, int perPage, Expression<Func<T, bool>> whereExp)
    {
        var count = await _entities.CountAsync();
        var entitiesToSkip = (page - 1) * perPage;
        var entities = await _entities
            .Where(whereExp)
            .OrderByDescending(ent => ent.UpdatedOn)
            .Skip(entitiesToSkip)
            .Take(perPage)
            .ToListAsync();

        return new PaginationResult<T>
        {
            TotalCount = count,
            Results = entities,
            ResultsPerPage = perPage,
            PageNumber = page
        };
    }

    /// <summary>
    /// Updates a resource in the database
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<T> Update(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        
        entity.UpdatedOn = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Deletes a resource from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<bool> Delete(Guid id)
    {
        try
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
            
            var entity = await _entities.SingleOrDefaultAsync(ent => ent.ID == id);
            _entities.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}