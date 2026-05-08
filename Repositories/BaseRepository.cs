using Giftify.Data;
using Giftify.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Giftify.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;

    public BaseRepository(ApplicationDbContext context)
    {
        this._context = context;
    }

    private IQueryable<T> ApplyIncludes(IQueryable<T> query, Expression<Func<T, object>>[] includes)
    {
        if (includes != null && includes.Any())
        {
            foreach (var include in includes)
                query = query.Include(include);
        }
        return query;
    }

    public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);
    public async Task<T?> FindAsync(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _context.Set<T>().Where(criteria).AsNoTracking();

        query = ApplyIncludes(query, includes);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<T?> FindAsync(Expression<Func<T, bool>> criteria, string[] includes)
    {
        IQueryable<T> query = _context.Set<T>().Where(criteria).AsNoTracking();

        if (includes != null)
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _context.Set<T>().AsNoTracking();

        query = ApplyIncludes(query, includes);

        return await query.ToListAsync();
    }
    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _context.Set<T>().Where(criteria).AsNoTracking();

        query = ApplyIncludes(query, includes);

        return await query.ToListAsync();
    }
    public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

    public void Update(T entity) => _context.Set<T>().Update(entity);
    public void Delete(T entity) => _context.Set<T>().Remove(entity);
}
