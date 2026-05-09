using Giftify.Interfaces.Services;
using Giftify.Models;
using Giftify.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Giftify.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IOccasionService _occasionService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IProductService productService,
                          ICategoryService categoryService,
                          IOccasionService occasionService,
                          ILogger<HomeController> logger)
    {
        _productService = productService;
        _categoryService = categoryService;
        _occasionService = occasionService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllProductsForCardsAsync();
        var categories = await _categoryService.GetAllCategoriesAsync();
        var occasions = await _occasionService.GetAllOccasionsAsync();

        var vm = new HomeVM
        {
            Occasions = occasions,
            Categories = categories,
            FeaturedProducts = products.Take(8)
        };

        return View(vm);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}