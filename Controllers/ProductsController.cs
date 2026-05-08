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
    public async Task<IActionResult> Index()
    {
        IEnumerable<ProductListItemVM> products = await _productService.GetAllProductsForCardsAsync();
        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    
    {
        ProductDetailsVM product = await _productService.GetProductDetailsAsync(id);
        return View(product);
    }
}
