using Giftify.Interfaces;
using Giftify.Interfaces.Services;
using Giftify.Models;
using Giftify.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Giftify.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin/Products/[action]/{id?}")]
public class AdminProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageUploadService _imageUpload;

    public AdminProductController(IUnitOfWork unitOfWork, IImageUploadService imageUpload)
    {
        _unitOfWork = unitOfWork;
        _imageUpload = imageUpload;
    }

    private const string ViewPath = "~/Views/Admin/AdminProduct/{0}.cshtml";

    // GET: /Admin  OR  /Admin/Products/Index
    [Route("/Admin")]
    [Route("/Admin/Products/Index")]
    public async Task<IActionResult> Index()
    {
        var products = await _unitOfWork.Products.GetAllAsync(p => p.Category, p => p.Images);
        return View(string.Format(ViewPath, "Index"), products);
    }

    // GET: Admin/Products/Create
    public async Task<IActionResult> Create()
    {
        await PopulateDropdownsAsync();
        return View(string.Format(ViewPath, "Create"), new AdminProductCreateVM());
    }

    // POST: Admin/Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AdminProductCreateVM vm)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(vm.CategoryId, vm.SelectedOccasionIds);
            return View(string.Format(ViewPath, "Create"), vm);
        }

        var imageUrl = await _imageUpload.UploadAsync(vm.ImageFile);

        var product = new Product
        {
            Name = vm.Name,
            Description = vm.Description,
            Price = vm.Price,
            Stock = vm.Stock,
            IsActive = vm.IsActive,
            CategoryId = vm.CategoryId,
            

        };

        if (imageUrl is not null)
        {
            product.Images.Add(new ProductImage
            {
                ImageUrl = imageUrl,
                IsPrimary = true,
                DisplayOrder = 1
            });
        }

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.CompleteAsync();

        // Link selected occasions
        if (vm.SelectedOccasionIds?.Any() == true)
        {
            foreach (var occId in vm.SelectedOccasionIds)
            {
                var occ = await _unitOfWork.Occasions.GetByIdAsync(occId);
                if (occ is not null)
                    occ.OccasionProducts.Add(new OccasionProduct { ProductId = product.Id, OccasionId = occId });
            }
            await _unitOfWork.CompleteAsync();
        }

        TempData["Success"] = $"Product '{product.Name}' created successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/Products/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _unitOfWork.Products.FindAsync(p => p.Id == id, p => p.Images, p => p.Category, p => p.OccasionProducts);
        if (product is null)
            return NotFound();

        var selectedOccasionIds = product.OccasionProducts?.Select(op => op.OccasionId).ToList() ?? new();
        await PopulateDropdownsAsync(product.CategoryId, selectedOccasionIds);

        var vm = new AdminProductEditVM
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId,
            SelectedOccasionIds = selectedOccasionIds,
            ExistingImageUrl = product.Images.FirstOrDefault(i => i.IsPrimary)?.ImageUrl
                               ?? product.Images.FirstOrDefault()?.ImageUrl
        };

        return View(string.Format(ViewPath, "Edit"), vm);
    }

    // POST: Admin/Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AdminProductEditVM vm)
    {
        if (id != vm.Id)
            return BadRequest();

        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(vm.CategoryId, vm.SelectedOccasionIds);
            return View(string.Format(ViewPath, "Edit"), vm);
        }

        var product = await _unitOfWork.Products.FindAsync(p => p.Id == id, p => p.Images, p => p.OccasionProducts);
        if (product is null)
            return NotFound();

        // Upload new image if provided
        if (vm.ImageFile is not null)
        {
            var newUrl = await _imageUpload.UploadAsync(vm.ImageFile);

            if (newUrl is not null)
            {
                // Delete old primary image file from disk
                var oldPrimary = product.Images.FirstOrDefault(i => i.IsPrimary);
                if (oldPrimary is not null)
                {
                    _imageUpload.Delete(oldPrimary.ImageUrl);
                    product.Images.Remove(oldPrimary);
                }

                product.Images.Add(new ProductImage
                {
                    ImageUrl = newUrl,
                    IsPrimary = true,
                    DisplayOrder = 1
                });
            }
        }

        product.Name = vm.Name;
        product.Description = vm.Description;
        product.Price = vm.Price;
        product.Stock = vm.Stock;
        product.IsActive = vm.IsActive;
        product.CategoryId = vm.CategoryId;

        // Update occasions: clear old ones, add newly selected
        product.OccasionProducts?.Clear();
        if (vm.SelectedOccasionIds?.Any() == true)
        {
            product.OccasionProducts ??= new List<OccasionProduct>();
            foreach (var occId in vm.SelectedOccasionIds)
                product.OccasionProducts.Add(new OccasionProduct { ProductId = product.Id, OccasionId = occId });
        }

        _unitOfWork.Products.Update(product);
        await _unitOfWork.CompleteAsync();

        TempData["Success"] = $"Product '{product.Name}' updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    // POST: Admin/Products/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _unitOfWork.Products.FindAsync(p => p.Id == id, p => p.Images);
        if (product is null)
            return NotFound();

        // Delete all image files from disk
        foreach (var img in product.Images)
            _imageUpload.Delete(img.ImageUrl);

        _unitOfWork.Products.Delete(product);
        await _unitOfWork.CompleteAsync();

        TempData["Success"] = $"Product '{product.Name}' deleted.";
        return RedirectToAction(nameof(Index));
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    private async Task PopulateDropdownsAsync(int? selectedCategoryId = null, IEnumerable<int>? selectedOccasionIds = null)
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedCategoryId);

        var occasions = await _unitOfWork.Occasions.GetAllAsync();
        ViewBag.Occasions = occasions;
        ViewBag.SelectedOccasionIds = selectedOccasionIds?.ToList() ?? new List<int>();
    }
}