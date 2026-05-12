using Giftify.Interfaces.Services;
using Giftify.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Giftify.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ICartService  _cartService;

    public OrderController(IOrderService orderService, ICartService cartService)
    {
        _orderService = orderService;
        _cartService  = cartService;
    }

    // GET /Order/Checkout
    public async Task<IActionResult> Checkout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var cart   = await _cartService.GetCartForUserAsync(userId);

        if (cart.IsEmpty)
            return RedirectToAction("Index", "Cart");

        // Map cart items → CheckoutItemVM for the order summary panel
        var vm = new CheckoutVM
        {
            Shipping = cart.ShippingEstimate,
            Items    = cart.Items.Select(i => new CheckoutItemVM
            {
                ProductId  = i.ProductId,
                Name       = i.ProductName,
                ImageUrl   = i.ImageUrl,
                Quantity   = i.Quantity,
                UnitPrice  = i.UnitPrice
            }).ToList()
        };

        return View(vm);
    }

    // POST /Order/Checkout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutVM vm)
    {
        // Re-populate items from cart (not submitted by the form)
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var cart   = await _cartService.GetCartForUserAsync(userId);

        vm.Items    = cart.Items.Select(i => new CheckoutItemVM
        {
            ProductId = i.ProductId,
            Name      = i.ProductName,
            ImageUrl  = i.ImageUrl,
            Quantity  = i.Quantity,
            UnitPrice = i.UnitPrice
        }).ToList();
        vm.Shipping = cart.ShippingEstimate;

        if (!ModelState.IsValid)
            return View(vm);

        try
        {
            var order = await _orderService.PlaceOrderAsync(userId, vm);
            TempData["OrderSuccess"] = order.Id;
            return RedirectToAction(nameof(Confirmation), new { id = order.Id });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(vm);
        }
    }

    // GET /Order/Confirmation/5
    public IActionResult Confirmation(int id)
    {
        ViewBag.OrderId = id;
        return View();
    }

    // GET /Order/MyOrders
    public async Task<IActionResult> MyOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var orders = await _orderService.GetOrdersByUserAsync(userId);
        return View(orders);
    }
}
