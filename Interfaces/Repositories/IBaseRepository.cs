using System.Linq.Expressions;

namespace Giftify.Interfaces.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<T?> FindAsync(Expression<Func<T, bool>> criteria, Expression<Func<T, object>>[]? includes = null);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>[]? includes = null);
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, Expression<Func<T, object>>[]? includes = null);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
