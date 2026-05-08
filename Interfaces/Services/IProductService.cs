using Giftify.ViewModels.Products;

namespace Giftify.Interfaces.Services;

public interface IProductService
{
    Task<IEnumerable<ProductListItemVM>> GetAllProductsForCardsAsync();
    Task<ProductDetailsVM> GetProductDetailsAsync(int productId);
}
