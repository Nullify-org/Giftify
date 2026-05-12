using Giftify.Interfaces;               // IUnitOfWork
using Giftify.Interfaces.Services;      // IAdminDashboardService
using Giftify.Models;
using Giftify.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;

namespace Giftify.Services;

public class AdminDashboardService : IAdminDashboardService
{
    private readonly IUnitOfWork                  _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminDashboardService(
        IUnitOfWork                  unitOfWork,
        UserManager<ApplicationUser> userManager)
    {
        _unitOfWork  = unitOfWork;
        _userManager = userManager;
    }

    public async Task<AdminDashboardVM> GetDashboardDataAsync()
    {
        var products   = await _unitOfWork.Products.GetAllAsync();
        var categories = await _unitOfWork.Categories.GetAllAsync();
        var occasions  = await _unitOfWork.Occasions.GetAllAsync();
        var orders     = (await _unitOfWork.Orders.GetAllOrdersWithUsersAsync()).ToList();
        var users      = _userManager.Users.ToList();

        return new AdminDashboardVM
        {
            TotalProducts    = products.Count(),
            TotalCategories  = categories.Count(),
            TotalOccasions   = occasions.Count(),
            TotalOrders      = orders.Count,
            TotalUsers       = users.Count,

            TotalRevenue     = orders
                                 .Where(o => o.Status != "Cancelled")
                                 .Sum(o => o.TotalAmount),

            PendingOrders    = orders.Count(o => o.Status == "Pending"),
            ProcessingOrders = orders.Count(o => o.Status == "Processing"),
            ShippedOrders    = orders.Count(o => o.Status == "Shipped"),
            DeliveredOrders  = orders.Count(o => o.Status == "Delivered"),

            RecentOrders     = orders
                                 .OrderByDescending(o => o.OrderDate)
                                 .Take(5)
                                 .ToList()
        };
    }
}
