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
    public IOccasionRepository Occasions { get; private set; }
    public ICategoryRepository Categories { get; private set; }

<<<<<<< HEAD
    public UnitOfWork(ApplicationDbContext context, ICartRepository cartRepo,
                        IOrderRepository orderRepo, IProductRepository productRepo,
                        ICategoryRepository categoryRepo, IOccasionRepository occasions)
=======
    public ICategoryRepository Categories { get; private set; }

    public UnitOfWork(ApplicationDbContext context, ICartRepository cartRepo, 
                        IOrderRepository orderRepo, IProductRepository productRepo , ICategoryRepository categoryRepo)
>>>>>>> 1e4876017e0475229b7cb5fd3cb178f81b5991d5
    {
        this._context = context;
        Carts = cartRepo;
        Orders = orderRepo;
        Products = productRepo;
        Categories = categoryRepo;
<<<<<<< HEAD
        Occasions = occasions;
    }



=======
        
    }

    
    
>>>>>>> 1e4876017e0475229b7cb5fd3cb178f81b5991d5
    public void Dispose()
    {
        _context?.Dispose();
    }

    public Task<int> Save()
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}
