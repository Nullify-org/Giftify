using System.Linq.Expressions;

namespace Giftify.Interfaces.Repositories;

public interface IBaseRepository<T>
{
    Task<T?> GetByIdAsync(int id);
    Task<T?> FindAsync(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[]? includes);
    Task<T?> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[]? includes);
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[]? includes);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
