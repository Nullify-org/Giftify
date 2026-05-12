using Giftify.Interfaces;
using Giftify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Giftify.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin/Occasions/[action]/{id?}")]
public class AdminOccasionController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public AdminOccasionController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    private const string VP = "~/Views/Admin/AdminOccasion/{0}.cshtml";

    public async Task<IActionResult> Index()
    {
        var occasions = await _unitOfWork.Occasions.GetAllAsync();
        ViewData["Title"] = "Occasions";
        return View(string.Format(VP, "Index"), occasions);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Add Occasion";
        return View(string.Format(VP, "Create"), new Occasion());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Occasion model)
    {
        if (!ModelState.IsValid)
            return View(string.Format(VP, "Create"), model);

        await _unitOfWork.Occasions.AddAsync(model);
        await _unitOfWork.Save();
        TempData["Success"] = $"Occasion '{model.Name}' created.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var occ = await _unitOfWork.Occasions.GetByIdAsync(id);
        if (occ is null) return NotFound();
        ViewData["Title"] = "Edit Occasion";
        return View(string.Format(VP, "Edit"), occ);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Occasion model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid)
            return View(string.Format(VP, "Edit"), model);

        _unitOfWork.Occasions.Update(model);
        await _unitOfWork.Save();
        TempData["Success"] = $"Occasion '{model.Name}' updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var occ = await _unitOfWork.Occasions.GetByIdAsync(id);
        if (occ is null) return NotFound();
        _unitOfWork.Occasions.Delete(occ);
        await _unitOfWork.Save();
        TempData["Success"] = $"Occasion '{occ.Name}' deleted.";
        return RedirectToAction(nameof(Index));
    }
}
