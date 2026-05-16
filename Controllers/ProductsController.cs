using Giftify.Interfaces.Services;
using Giftify.ViewModels.Categories;
using Giftify.ViewModels.Occasions;
using Giftify.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding;
using System.Threading.Tasks;

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
        ProductIndexVM productIndex = await _productService.GetFilteredProductsAsync(filter);
        return View(productIndex);
    }

    public async Task<IActionResult> Details(int id)
    {
        ProductDetailsVM product = await _productService.GetProductDetailsAsync(id);
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        CreateProductVM model = await _productService.GetCreateProductVMAsync();
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductVM model)
    {
        if (ModelState.IsValid)
        {
            await _productService.CreateProductAsync(model);

            return RedirectToAction("Index");
        }
        var refreshModel = await _productService.GetCreateProductVMAsync();

        model.Categories = refreshModel.Categories;
        model.Occasions = refreshModel.Occasions;

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        CreateProductVM model = await _productService.GetProductForEditAsync(id);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditProductVM model)
    {
        if (ModelState.IsValid)
        {
            await _productService.UpdateProductAsync(model);
            return RedirectToAction("Index");
        }

        model = await _productService.GetProductForEditAsync(model.Id);

        return View(model);
    }

    public async Task<IActionResult> DeleteImage(int imageId)
    {
        try
        {
            await _productService.DeleteProductImage(imageId);
            return Json(new { success = true, message = "Image deleted successfully!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "An error occurred while deleting the image." });

        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteProductAsync(id);
        return RedirectToAction("Index");
    }
}
