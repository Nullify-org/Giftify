using Giftify.Data;
using Giftify.Interfaces.Repositories;
using Giftify.Models;

namespace Giftify.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) : base(context)
    {

    }

}
