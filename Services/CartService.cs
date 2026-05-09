using Giftify.Interfaces;
using Giftify.Interfaces.Services;
using Giftify.Models;
using Giftify.ViewModels.Cart;

namespace Giftify.Services;

public class CartService : ICartService
{
    private readonly IUnitOfWork _uow;

    public CartService(IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
    }


    public async Task<CartVM> GetCartAsync(string userId)
    {
        var cart = await GetOrCreateCartAsync(userId);
        return MapToVM(cart);
    }

    public async Task<int> GetCartItemCountAsync(string userId)
    {
        var cart = await _uow.Carts.GetCartByUserIdAsync(userId);
        return cart?.CartItems.Sum(ci => ci.Quantity) ?? 0;
    }


    public async Task AddToCartAsync(string userId, int productId, int quantity = 1)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be at least 1.");

        var cart = await GetOrCreateCartAsync(userId);
        var product = await _uow.Products.GetByIdAsync(productId)
                      ?? throw new KeyNotFoundException($"Product {productId} not found.");

        var existing = await _uow.Carts.GetCartItemAsync(cart.Id, productId);

        if (existing is not null)
        {
            existing.Quantity += quantity;
            _uow.Carts.UpdateCartItem(existing);
        }
        else
        {
            await _uow.Carts.AddCartItemAsync(new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = product.Price
            });
        }

        await _uow.Save();
    }

    public async Task UpdateQuantityAsync(string userId, int productId, int quantity)
    {
        if (quantity <= 0)
        {
            await RemoveFromCartAsync(userId, productId);
            return;
        }

        var cart = await _uow.Carts.GetCartByUserIdAsync(userId)
                   ?? throw new KeyNotFoundException("Cart not found.");

        var item = await _uow.Carts.GetCartItemAsync(cart.Id, productId)
                   ?? throw new KeyNotFoundException("Item not in cart.");

        item.Quantity = quantity;
        _uow.Carts.UpdateCartItem(item);
        await _uow.Save();
    }

    public async Task RemoveFromCartAsync(string userId, int productId)
    {
        var cart = await _uow.Carts.GetCartByUserIdAsync(userId)
                   ?? throw new KeyNotFoundException("Cart not found.");

        var item = await _uow.Carts.GetCartItemAsync(cart.Id, productId)
                   ?? throw new KeyNotFoundException("Item not in cart.");

        _uow.Carts.RemoveCartItem(item);
        await _uow.Save();
    }

    public async Task ClearCartAsync(string userId)
    {
        var cart = await _uow.Carts.GetCartByUserIdAsync(userId);
        if (cart is null) return;

        await _uow.Carts.ClearCartAsync(cart.Id);
        await _uow.Save();
    }


    private async Task<Cart> GetOrCreateCartAsync(string userId)
    {
        var cart = await _uow.Carts.GetCartByUserIdAsync(userId);

        if (cart is null)
        {
            cart = new Cart { ApplicationUserId = userId, CreatedAt = DateTime.Now };
            await _uow.Carts.AddAsync(cart);
            await _uow.Save();
            cart = (await _uow.Carts.GetCartByUserIdAsync(userId))!;
        }

        return cart!;
    }

    private static CartVM MapToVM(Cart cart) => new()
    {
        CartId = cart.Id,
        UserId = cart.ApplicationUserId,
        Items = cart.CartItems.Select(ci => new CartItemVM
        {
            CartItemId = ci.Id,
            ProductId = ci.ProductId,
            ProductName = ci.Product?.Name ?? string.Empty,
            ImageUrl = ci.Product?.Images?
                            .FirstOrDefault(i => i.IsPrimary)?.ImageUrl
                          ?? ci.Product?.Images?.FirstOrDefault()?.ImageUrl,
            UnitPrice = ci.UnitPrice,
            Quantity = ci.Quantity,
            IsInStock = (ci.Product?.Stock ?? 0) > 0
        }).ToList()
    };
}