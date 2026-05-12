using Giftify.ViewModels.Products;

namespace Giftify.Interfaces.Services;

public interface IProductService
{
    Task<ProductIndexVM> GetAllProductsForIndexAsync();
    Task<ProductDetailsVM> GetProductDetailsAsync(int productId);
    Task<ProductIndexVM> GetFilteredProductsAsync(ProductFilterVM model);
    Task<CreateProductVM> GetCreateProductVMAsync();
    Task CreateProductAsync(CreateProductVM model);
    Task<EditProductVM> GetProductForEditAsync(int productId);
    Task UpdateProductAsync(EditProductVM model);
    Task DeleteProductAsync(int productId);
    Task DeleteProductImage(int imageId);
}
