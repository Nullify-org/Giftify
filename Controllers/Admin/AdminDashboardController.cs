using Giftify.Interfaces.Services;      // IAdminDashboardService
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Giftify.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin/[action]")]
public class AdminDashboardController : Controller
{
    private readonly IAdminDashboardService _dashboardService;

    public AdminDashboardController(IAdminDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [Route("/Admin")]
    [Route("/Admin/Index")]
    public async Task<IActionResult> Index()
    {
        var vm = await _dashboardService.GetDashboardDataAsync();
        ViewData["Title"] = "Dashboard";
        return View("~/Views/Admin/Dashboard/Index.cshtml", vm);
    }
}
