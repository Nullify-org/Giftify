using Giftify.Models;
using System.Linq.Expressions;

namespace Giftify.Interfaces.Repositories
{
<<<<<<< HEAD
    public interface ICategoryRepository : IBaseRepository<Category>
    {
      
=======
    public class ICategoryRepository : IBaseRepository<Category>
    {
        public Task AddAsync(Category entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> FindAllAsync(Expression<Func<Category, bool>> criteria, params Expression<Func<Category, object>>[]? includes)
        {
            throw new NotImplementedException();
        }

        public Task<Category?> FindAsync(Expression<Func<Category, bool>> criteria, params Expression<Func<Category, object>>[]? includes)
        {
            throw new NotImplementedException();
        }

        public Task<Category?> FindAsync(Expression<Func<Category, bool>> criteria, string[] includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetAllAsync(params Expression<Func<Category, object>>[]? includes)
        {
            throw new NotImplementedException();
        }

        public Task<Category?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Category entity)
        {
            throw new NotImplementedException();
        }
>>>>>>> 1e4876017e0475229b7cb5fd3cb178f81b5991d5
    }
}
