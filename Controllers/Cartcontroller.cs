using Giftify.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Giftify.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var cart = await _cartService.GetCartAsync(userId);
        return View(cart);
    }

  
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        bool isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        try
        {
            await _cartService.AddToCartAsync(userId, productId, quantity);
            var count = await _cartService.GetCartItemCountAsync(userId);

            if (isAjax)
                return Json(new { success = true, cartCount = count, message = "Item added to cart!" });

            TempData["Success"] = "Item added to cart!";
        }
        catch (KeyNotFoundException)
        {
            if (isAjax)
                return Json(new { success = false, message = "Product not found." });

            TempData["Error"] = "Product not found.";
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Remove(int productId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        try
        {
            await _cartService.RemoveFromCartAsync(userId, productId);
            TempData["Success"] = "Item removed from cart.";
        }
        catch (KeyNotFoundException)
        {
            TempData["Error"] = "Item not found in cart.";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        try
        {
            await _cartService.UpdateQuantityAsync(userId, productId, quantity);
        }
        catch (KeyNotFoundException)
        {
            TempData["Error"] = "Item not found in cart.";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Clear()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _cartService.ClearCartAsync(userId);
        TempData["Success"] = "Cart cleared.";
        return RedirectToAction("Index");
    }
}
