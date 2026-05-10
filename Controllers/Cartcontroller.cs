using Giftify.Interfaces.Services;
using Giftify.ViewModels.Cart;
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

    // GET /Cart
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cart = await _cartService.GetCartForUserAsync(userId);
        return View(cart);
    }

    // POST /Cart/Add
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add([FromBody] AddToCartVM request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, message = "Invalid request." });

        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartService.AddItemAsync(userId, request);
            var count = await _cartService.GetCartItemCountAsync(userId);
            return Ok(new { success = true, cartCount = count, message = "Item added to cart!" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // Stock validation errors (out of stock, exceeds available qty)
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    // POST /Cart/UpdateQuantity
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuantity([FromBody] UpdateCartItemVM request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, message = "Invalid request." });

        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartService.UpdateItemQuantityAsync(userId, request);
            var count = await _cartService.GetCartItemCountAsync(userId);
            return Ok(new { success = true, cartCount = count });
        }
        catch (InvalidOperationException ex)
        {
            // Stock validation errors — tell JS to revert the input
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // POST /Cart/Remove
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove([FromBody] RemoveCartItemRequest request)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartService.RemoveItemAsync(userId, request.CartItemId);
            var count = await _cartService.GetCartItemCountAsync(userId);
            return Ok(new { success = true, cartCount = count });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // POST /Cart/Clear
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Clear()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _cartService.ClearCartAsync(userId);
        return Ok(new { success = true, cartCount = 0 });
    }

    // GET /Cart/Count
    [HttpGet]
    public async Task<IActionResult> Count()
    {
        if (!User.Identity.IsAuthenticated)
            return Ok(new { cartCount = 0 });

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var count = await _cartService.GetCartItemCountAsync(userId);
        return Ok(new { cartCount = count });
    }
}

public class RemoveCartItemRequest
{
    public int CartItemId { get; set; }
}