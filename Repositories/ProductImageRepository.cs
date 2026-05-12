using Giftify.Data;
using Giftify.Interfaces.Repositories;
using Giftify.Models;

namespace Giftify.Repositories;

public class ProductImageRepository : BaseRepository<ProductImage>, IProductImageRepository
{
    private readonly ApplicationDbContext context;
    public ProductImageRepository(ApplicationDbContext context) : base(context)
    {
        this.context = context;
    }
}
