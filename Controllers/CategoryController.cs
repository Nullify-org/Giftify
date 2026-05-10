using Giftify.Interfaces;
using Giftify.Models;
using Giftify.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;

namespace Giftify.Controllers;

public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index(int id)
    {
        string[] includes = { nameof(Category.Products) };
        var category = await _unitOfWork.Categories.FindAsync(c => c.Id == id, includes);

        if (category == null)
            return NotFound();

        var categoryProducts = new List<CategoryProductItemVM>();
        foreach (var p in category.Products ?? Enumerable.Empty<Product>())
        {
            var fullProduct = await _unitOfWork.Products
                .FindAsync(pr => pr.Id == p.Id, pr => pr.Images);

            categoryProducts.Add(new CategoryProductItemVM
            {
                Id       = p.Id,
                Name     = p.Name,
                Price    = p.Price,
                ImageUrl = fullProduct?.Images?.FirstOrDefault(i => i.IsPrimary)?.ImageUrl
                           ?? fullProduct?.Images?.FirstOrDefault()?.ImageUrl
                           ?? "/images/default-product.png",
                IsInStock = p.Stock > 0,
                Stock    = p.Stock
            });
        }

        var allCategories = await _unitOfWork.Categories.GetAllAsync(c => c.Products);
        var relatedCategories = allCategories
            .Where(c => c.Id != id && c.IsActive)
            .Select(c => new RelatedCategoryVM
            {
                Id           = c.Id,
                Name         = c.Name,
                ProductCount = c.Products?.Count ?? 0
            }).ToList();

        var vm = new CategoryLandingVM
        {
            CategoryId          = category.Id,
            CategoryName        = category.Name,
            CategoryDescription = category.Description,
            TotalProducts       = categoryProducts.Count,
            Products            = categoryProducts,
            RelatedCategories   = relatedCategories
        };

        return View(vm);
    }
}
