using Giftify.Interfaces.Repositories;

namespace Giftify.Interfaces;

public interface IUnitOfWork : IDisposable
{
<<<<<<< HEAD
    ICartRepository Carts { get; }
    IOrderRepository Orders { get; }
    IProductRepository Products { get; }
    IOccasionRepository Occasions { get; }
=======
     ICartRepository Carts { get; }
     IOrderRepository Orders { get; }
     IProductRepository Products { get; }

>>>>>>> 1e4876017e0475229b7cb5fd3cb178f81b5991d5
    ICategoryRepository Categories { get; }
    Task<int> Save();
    Task SaveChangesAsync();
}
