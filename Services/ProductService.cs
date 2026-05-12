using Giftify.Interfaces;
using Giftify.Interfaces.Services;
using Giftify.Models;
using Giftify.ViewModels.Categories;
using Giftify.ViewModels.Occasions;
using Giftify.ViewModels.Products;
using Microsoft.Build.Framework;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

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

    public async Task<CreateProductVM> GetCreateProductVMAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        var occasions = await _unitOfWork.Occasions.GetAllAsync();

        return new CreateProductVM()
        {
            Categories = categories.Select(c => new CategoryVM { Id = c.Id, Name = c.Name }).ToList(),
            Occasions = occasions.Select(o => new OccasionVM { Id = o.Id, Name = o.Name }).ToList(),
        };

    }
    public async Task CreateProductAsync(CreateProductVM model)
    {
        var product = new Product
        {
            Name = model.Name,
            Description = model.Description ?? "",
            Price = model.Price,
            Stock = model.Stock,
            IsActive = model.Stock > 0,
            CategoryId = model.CategoryId,
            OccasionProducts = model.SelectedOccasionIds != null ? model.SelectedOccasionIds
                                .Select(o => new OccasionProduct { OccasionId = o }).ToList() : new(),
        };

        var imagePaths = await UploadImagesAsync(model.Images);

        product.Images = imagePaths.Select(i => new ProductImage { ImageUrl = i }).ToList();

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<EditProductVM> GetProductForEditAsync(int productId)
    {
        var product = await _unitOfWork.Products.FindAsync(p => p.Id == productId, p => p.Images, p => p.OccasionProducts);
        var categories = await _unitOfWork.Categories.GetAllAsync();

        var lookups = await GetCreateProductVMAsync();

        return new EditProductVM
        {
            Id = productId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CategoryId = product.CategoryId,
            SelectedOccasionIds = product.OccasionProducts.Select(op => op.OccasionId).ToList(),
            ExistingImages = product.Images.Select(img => new ProductImageVM { Id = img.Id, Url = img.ImageUrl }).ToList(),
            Categories = lookups.Categories,
            Occasions = lookups.Occasions,
        };
    }

    public async Task UpdateProductAsync(EditProductVM model)
    {
        var product = await _unitOfWork.Products
                        .FindAsync(p => p.Id == model.Id, new[] { "Images", "OccasionProducts" }, true);

        if (product == null)
            return;

        product.Name = model.Name;
        product.Description = model.Description;
        product.Price = model.Price;
        product.Stock = model.Stock;
        product.IsActive = model.Stock > 0;
        product.CategoryId = model.CategoryId;

        product.OccasionProducts.Clear();

        if (model.SelectedOccasionIds != null)
        {
            foreach (var occId in model.SelectedOccasionIds)
            {
                product.OccasionProducts.Add(new OccasionProduct
                {
                    OccasionId = occId,
                    ProductId = product.Id
                });
            }
        }

        var imagePaths = await UploadImagesAsync(model.Images);
        if (imagePaths.Any())
        {
            foreach (var path in imagePaths)
            {
                product.Images.Add(new ProductImage
                {
                    ImageUrl = path,
                });
            }
        }
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteProductImage(int imageId)
    {
        var image = await _unitOfWork.ProductImages.GetByIdAsync(imageId);

        if (image != null)
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImageUrl.TrimStart('/'));

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            _unitOfWork.ProductImages.Delete(image);
            await _unitOfWork.CompleteAsync();
        }
    }

    public async Task DeleteProductAsync(int productId)
    {
        var product = await _unitOfWork.Products.FindAsync(p => p.Id == productId, p => p.Images);

        if (product == null)
            return;

        if (product.Images != null && product.Images.Any())
        {
            foreach (var image in product.Images)
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
        }

        _unitOfWork.Products.Delete(product);
        await _unitOfWork.CompleteAsync();
    }
    private async Task<List<string>> UploadImagesAsync(IEnumerable<IFormFile> images)
    {
        var uploadedImages = new List<string>();
        if (images != null && images.Any())
        {
            string folderName = "images/products";
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            foreach (var file in images)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string fullPath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                uploadedImages.Add($"/{folderName}/{fileName}");
            }
        }
        return uploadedImages;
    }
}
