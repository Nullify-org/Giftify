using Giftify.Enums;
using Giftify.Interfaces;               // IUnitOfWork
using Giftify.Interfaces.Services;      // IOrderService
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Giftify.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("Admin/Orders/[action]/{id?}")]
public class AdminOrderController : Controller
{
    private readonly IUnitOfWork   _unitOfWork;
    private readonly IOrderService _orderService;

    public AdminOrderController(IUnitOfWork unitOfWork, IOrderService orderService)
    {
        _unitOfWork   = unitOfWork;
        _orderService = orderService;
    }

    private const string VP = "~/Views/Admin/AdminOrder/{0}.cshtml";

    public async Task<IActionResult> Index(string? status = null)
    {
        var orders = string.IsNullOrEmpty(status)
            ? await _unitOfWork.Orders.GetAllOrdersWithUsersAsync()
            : await _unitOfWork.Orders.GetOrdersByStatusAsync(status);

        ViewBag.StatusFilter = status;
        ViewBag.Statuses     = Enum.GetNames<OrderStatus>();
        ViewData["Title"]    = "Orders";
        return View(string.Format(VP, "Index"), orders.OrderByDescending(o => o.OrderDate).ToList());
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = await _unitOfWork.Orders.GetOrderWithDetailsAsync(id);
        if (order is null) return NotFound();

        ViewBag.Statuses  = Enum.GetNames<OrderStatus>();
        ViewData["Title"] = $"Order #{id}";
        return View(string.Format(VP, "Details"), order);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        await _orderService.UpdateOrderStatusAsync(id, status);
        TempData["Success"] = $"Order #{id} status updated to '{status}'.";
        return RedirectToAction(nameof(Details), new { id });
    }
}
