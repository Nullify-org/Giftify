using Giftify.Interfaces.Services;      // IAdminUserService
using Giftify.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Giftify.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin/Users/[action]/{id?}")]
public class AdminUserController : Controller
{
    private readonly IAdminUserService _userService;

    public AdminUserController(IAdminUserService userService)
    {
        _userService = userService;
    }

    private const string VP = "~/Views/Admin/AdminUser/{0}.cshtml";

    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllUsersAsync();
        ViewData["Title"] = "Manage Users";
        return View(string.Format(VP, "Index"), users);
    }

    public async Task<IActionResult> Create()
    {
        var roles = await _userService.GetAvailableRolesAsync();
        ViewBag.Roles = new SelectList(roles);
        ViewData["Title"] = "Add User";
        return View(string.Format(VP, "Create"), new AdminUserCreateVM());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AdminUserCreateVM vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = new SelectList(await _userService.GetAvailableRolesAsync());
            return View(string.Format(VP, "Create"), vm);
        }

        var result = await _userService.CreateUserAsync(vm);

        if (!result.Succeeded)
        {
            foreach (var err in result.Errors)
                ModelState.AddModelError(string.Empty, err.Description);

            ViewBag.Roles = new SelectList(await _userService.GetAvailableRolesAsync());
            return View(string.Format(VP, "Create"), vm);
        }

        TempData["Success"] = $"User '{vm.UserName}' created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _userService.DeleteUserAsync(id, User.Identity?.Name ?? "");

        if (!result.Succeeded)
            TempData["Error"] = result.Errors.FirstOrDefault()?.Description;
        else
            TempData["Success"] = "User deleted successfully.";

        return RedirectToAction(nameof(Index));
    }
}
