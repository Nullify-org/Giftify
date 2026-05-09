using Giftify.ViewModels.Products;

namespace Giftify.Interfaces.Services;

public interface IProductService
{
    Task<ProductIndexVM> GetAllProductsForIndexAsync();
    Task<ProductDetailsVM> GetProductDetailsAsync(int productId);
    Task<ProductIndexVM> GetFilteredProductsAsync(ProductFilterVM model);
}
