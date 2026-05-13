using Giftify.Interfaces;
using Giftify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Giftify.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin/Categories/[action]/{id?}")]
public class AdminCategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public AdminCategoryController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    private const string VP = "~/Views/Admin/AdminCategory/{0}.cshtml";

    public async Task<IActionResult> Index()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        ViewData["Title"] = "Categories";
        return View(string.Format(VP, "Index"), categories);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Add Category";
        return View(string.Format(VP, "Create"), new Category());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category model)
    {
        if (!ModelState.IsValid)
            return View(string.Format(VP, "Create"), model);

        await _unitOfWork.Categories.AddAsync(model);
        await _unitOfWork.CompleteAsync();
        TempData["Success"] = $"Category '{model.Name}' created.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var cat = await _unitOfWork.Categories.GetByIdAsync(id);
        if (cat is null) return NotFound();
        ViewData["Title"] = "Edit Category";
        return View(string.Format(VP, "Edit"), cat);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid)
            return View(string.Format(VP, "Edit"), model);

        _unitOfWork.Categories.Update(model);
        await _unitOfWork.CompleteAsync();
        TempData["Success"] = $"Category '{model.Name}' updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var cat = await _unitOfWork.Categories.GetByIdAsync(id);
        if (cat is null) return NotFound();
        _unitOfWork.Categories.Delete(cat);
        await _unitOfWork.CompleteAsync();
        TempData["Success"] = $"Category '{cat.Name}' deleted.";
        return RedirectToAction(nameof(Index));
    }
}
