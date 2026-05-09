using Giftify.Interfaces.Repositories;
using Giftify.Repositories;

namespace Giftify.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICartRepository Carts { get; }
    IOrderRepository Orders { get; }
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }
    IOccasionRepository Occasions { get; }
    Task<int> Save();
    Task SaveChangesAsync();
}
