using Giftify.Interfaces.Services;
using Giftify.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;

namespace Giftify.Controllers;
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
        this._productService = productService;
    }
    public async Task<IActionResult> Index(ProductFilterVM filter)
    {
<<<<<<< HEAD
        ProductIndexVM productIndex = await _productService.GetFilteredProductsAsync(filter);
        return View(productIndex);
=======


        IEnumerable<ProductListItemVM> products = await _productService.GetAllProductsForCardsAsync();
        return View(products);
>>>>>>> 1e4876017e0475229b7cb5fd3cb178f81b5991d5
    }

    public async Task<IActionResult> Details(int id)
    {
        ProductDetailsVM product = await _productService.GetProductDetailsAsync(id);
        return View(product);
    }
}
