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

    public UnitOfWork(ApplicationDbContext context)
    {
        this._context = context;
        Carts = new CartRepository(_context);
        Orders = new OrderRepository(_context);
        Products = new ProductRepository(_context);
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
