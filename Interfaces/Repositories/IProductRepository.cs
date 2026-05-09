using Giftify.Models;
using Giftify.ViewModels.Products;

namespace Giftify.Interfaces.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    Task <IEnumerable<Product>> SearchAsync(ProductFilterVM model);
}
