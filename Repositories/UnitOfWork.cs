using Giftify.Data;
using Giftify.Interfaces;
using Giftify.Interfaces.Repositories;

namespace Giftify.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public ICartRepository Carts { get; private set; }
    public IOrderRepository Orders { get; private set; }
    public IProductRepository Products { get; private set; }

    public UnitOfWork(ApplicationDbContext context, ICartRepository cartRepo, 
                        IOrderRepository orderRepo, IProductRepository productRepo)
    {
        this._context = context;
        Carts = cartRepo;
        Orders = orderRepo;
        Products = productRepo;
    }

    public async Task<int> Save()
    {
        return await _context.SaveChangesAsync();
    }
    public void Dispose()
    {
        _context?.Dispose();
    }
}
