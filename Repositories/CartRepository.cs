using Giftify.Data;
using Giftify.Interfaces.Repositories;
using Giftify.Models;

namespace Giftify.Repositories;

public class CartRepository : BaseRepository<Cart>, ICartRepository
{
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context) : base(context)
    {

    }

}
