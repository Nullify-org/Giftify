using Giftify.Interfaces;
using Giftify.Interfaces.Services;
using Giftify.Models;
using Giftify.ViewModels.Categories;
using Giftify.ViewModels.Occasions;
using Giftify.ViewModels.Products;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NuGet.Protocol;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Giftify.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public async Task<ProductIndexVM> GetAllProductsForIndexAsync()
    {
        var products = await _unitOfWork.Products.GetAllAsync(p => p.Images);
        var categoris = await _unitOfWork.Categories.GetAllAsync();
        var occasions = await _unitOfWork.Occasions.GetAllAsync();

        var productsVM = products.Select(p =>
        new ProductListItemVM
        {
            Id = p.Id,
            Name = p.Name,
            ImageUrl = p.Images.FirstOrDefault()?.ImageUrl ?? "default-image.jpg",
            IsInStock = p.Stock > 0,
            Price = p.Price,
        });

        ProductIndexVM index = new ProductIndexVM
        {
            Products = productsVM,
            Categories = categoris.Select(c => new CategoryVM { Id = c.Id, Name = c.Name }).ToList(),
            Occasions = occasions.Select(o => new OccasionVM { Id = o.Id, Name = o.Name }).ToList()
        };
        return index;
    }

    public async Task<ProductDetailsVM> GetProductDetailsAsync(int productId)
    {

        string[] includes =
        {
            nameof(Product.Images),
            nameof(Product.Category),
            $"{nameof(Product.OccasionProducts)}.{nameof(OccasionProduct.Occasion)}"
        };

        var product = await _unitOfWork.Products
            .FindAsync(p => p.Id == productId, includes);

        if (product == null)
            return null;

        var productDetailsVM = new ProductDetailsVM
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = new CategoryVM { Id = product.Category.Id, Name = product.Category.Name },
            ImageUrls = product.Images?
                       .Select(p => p.ImageUrl).ToList() ?? new List<string>(),
            StockQuantity = product.Stock,
            Occasions = product.OccasionProducts
                    .Select(op => new OccasionVM
                    {
                        Id = op.Occasion.Id,
                        Name = op.Occasion.Name
                    }).ToList()
        };
        return productDetailsVM;
    }

    public async Task<ProductIndexVM> GetFilteredProductsAsync(ProductFilterVM model)
    {
        var products = await _unitOfWork.Products.SearchAsync(model);
        var categories = await _unitOfWork.Categories.GetAllAsync();
        var occasions = await _unitOfWork.Occasions.GetAllAsync();

        var result = new ProductIndexVM
        {
            Products = products.Select(p => new ProductListItemVM
            {
                Id = p.Id,
                Name = p.Name,
                ImageUrl = p.Images?.FirstOrDefault()?.ImageUrl ?? "default-image.jpg",
                IsInStock = p.Stock > 0,
                Price = p.Price
            }),
            Categories = categories.Select(c => new CategoryVM { Id = c.Id, Name = c.Name }).ToList(),
            Occasions = occasions.Select(o => new OccasionVM { Id = o.Id, Name = o.Name }).ToList(),
            CurrentFilters = model
        };
        return result;
    }
}
