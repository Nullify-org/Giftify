using Giftify.Interfaces.Services;
using Giftify.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;

namespace Giftify.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly IOccasionService _occasionService;

    public ProductsController(IProductService productService, IOccasionService occasionService)
    {
        _productService = productService;
        _occasionService = occasionService;
    }

    public async Task<IActionResult> Index(int? occasionId = null, int? categoryId = null)
    {
        var products = await _productService.GetAllProductsForCardsAsync();

        if (occasionId.HasValue)
            products = products.Where(p => p.OccasionIds != null && p.OccasionIds.Contains(occasionId.Value));

        if (categoryId.HasValue)
            products = products.Where(p => p.CategoryId == categoryId.Value);

        ViewBag.OccasionId = occasionId;
        ViewBag.CategoryId = categoryId;
        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        ProductDetailsVM product = await _productService.GetProductDetailsAsync(id);
        return View(product);
    }

    public async Task<IActionResult> Occasions()
    {
        var occasions = await _occasionService.GetAllOccasionsAsync();
        return View("~/Views/occassions/Occasions.cshtml", occasions);
    }
}