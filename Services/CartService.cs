using Giftify.Interfaces;
using Giftify.Interfaces.Services;
using Giftify.Models;
using Giftify.ViewModels.Cart;
using Microsoft.EntityFrameworkCore;

namespace Giftify.Services;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;

    public CartService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // ─── Read ────────────────────────────────────────────────────────────────

    public async Task<CartVM> GetCartForUserAsync(string userId)
    {
        var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);

        if (cart == null)
            return new CartVM { UserId = userId };

        return MapToCartVM(cart, userId);
    }

    public async Task<int> GetCartItemCountAsync(string userId)
    {
        var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);
        return cart?.CartItems?.Sum(i => i.Quantity) ?? 0;
    }

    // ─── Write ───────────────────────────────────────────────────────────────

    public async Task AddItemAsync(string userId, AddToCartVM request)
    {
        // 1. Get or create the user's cart (tracked)
        var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);

        if (cart == null)
        {
            cart = new Cart { ApplicationUserId = userId };
            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.Save(); // need Cart.Id before inserting items

            // Reload so CartItems collection is initialised
            cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);
        }

        // 2. Fetch the product to validate stock
        var product = await _unitOfWork.Products
            .FindAsync(p => p.Id == request.ProductId);

        if (product == null)
            throw new KeyNotFoundException($"Product {request.ProductId} not found.");

        if (product.Stock == 0)
            throw new InvalidOperationException($"Sorry, \"{product.Name}\" is out of stock.");

        // 3. Check if product already in cart — merge quantities
        var existingItem = cart!.CartItems?
            .FirstOrDefault(ci => ci.ProductId == request.ProductId);

        if (existingItem != null)
        {
            int newQty = existingItem.Quantity + request.Quantity;

            if (newQty > product.Stock)
                throw new InvalidOperationException(
                    $"Only {product.Stock} unit(s) of \"{product.Name}\" available. " +
                    $"You already have {existingItem.Quantity} in your cart.");

            existingItem.Quantity = newQty;
        }
        else
        {
            if (request.Quantity > product.Stock)
                throw new InvalidOperationException(
                    $"Only {product.Stock} unit(s) of \"{product.Name}\" available.");

            var newItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UnitPrice = product.Price   // snapshot price at time of adding
            };

            await _unitOfWork.Carts.AddCartItemAsync(newItem);
        }

        await _unitOfWork.Save();
    }

    public async Task UpdateItemQuantityAsync(string userId, UpdateCartItemVM request)
    {
        // Load cart first (tracked), then find the item inside it
        var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId)
            ?? throw new InvalidOperationException("No cart found for this user.");

        var item = cart.CartItems?.FirstOrDefault(ci => ci.Id == request.CartItemId)
            ?? throw new KeyNotFoundException($"Cart item {request.CartItemId} not found.");

        // Validate against current stock
        var product = await _unitOfWork.Products
            .FindAsync(p => p.Id == item.ProductId);

        if (product == null)
            throw new KeyNotFoundException($"Product not found.");

        if (product.Stock == 0)
            throw new InvalidOperationException(
                $"Sorry, \"{product.Name}\" is out of stock.");

        if (request.NewQuantity > product.Stock)
            throw new InvalidOperationException(
                $"Only {product.Stock} unit(s) of \"{product.Name}\" available.");

        item.Quantity = request.NewQuantity;
        await _unitOfWork.Save();
    }

    public async Task RemoveItemAsync(string userId, int cartItemId)
    {
        // Load cart (tracked), find the item inside it
        var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId)
            ?? throw new InvalidOperationException("No cart found for this user.");

        var item = cart.CartItems?.FirstOrDefault(ci => ci.Id == cartItemId)
            ?? throw new KeyNotFoundException($"Cart item {cartItemId} not found.");

        _unitOfWork.Carts.RemoveCartItem(item);
        await _unitOfWork.Save();
    }

    public async Task ClearCartAsync(string userId)
    {
        var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);
        if (cart == null) return;

        await _unitOfWork.Carts.ClearCartItemsAsync(cart.Id);
        await _unitOfWork.Save();
    }

    // ─── Private helpers ─────────────────────────────────────────────────────

    private static CartVM MapToCartVM(Cart cart, string userId)
    {
        var items = cart.CartItems?.Select(ci => new CartItemVM
        {
            CartItemId = ci.Id,
            ProductId = ci.ProductId,
            ProductName = ci.Product?.Name ?? "Unknown",
            ImageUrl = ci.Product?.Images?.FirstOrDefault(i => i.IsPrimary)?.ImageUrl
                          ?? ci.Product?.Images?.FirstOrDefault()?.ImageUrl
                          ?? "/images/default-product.png",
            UnitPrice = ci.UnitPrice,
            Quantity = ci.Quantity,
            Stock = ci.Product?.Stock ?? 0   // expose stock so the view can cap the input
        }).ToList() ?? new List<CartItemVM>();

        return new CartVM
        {
            CartId = cart.Id,
            UserId = userId,
            Items = items
        };
    }
}