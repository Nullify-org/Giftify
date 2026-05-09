using Giftify.Data;
using Giftify.Interfaces.Repositories;
using Giftify.Models;
using Microsoft.Identity.Client;

namespace Giftify.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}