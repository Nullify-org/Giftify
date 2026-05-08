using Giftify.Interfaces.Repositories;

namespace Giftify.Interfaces;

public interface IUnitOfWork : IDisposable
{
     ICartRepository Carts { get; }
     IOrderRepository Orders { get; }
     IProductRepository Products { get; }

    Task<int> Save();
}
