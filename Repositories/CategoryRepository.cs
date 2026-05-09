using Giftify.Data;
using Giftify.Interfaces.Repositories;
using Giftify.Models;

namespace Giftify.Repositories;

    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }

   
