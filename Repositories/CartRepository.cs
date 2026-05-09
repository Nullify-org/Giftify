using Giftify.Data;
using Giftify.Interfaces.Repositories;
using Giftify.Models;
using Microsoft.EntityFrameworkCore;

namespace Giftify.Repositories;

public class CartRepository : BaseRepository<Cart>, ICartRepository
{
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Cart?> GetCartByUserIdAsync(string userId)
    {
        return await _context.Carts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                    .ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);
    }

    public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
    }

    public async Task AddCartItemAsync(CartItem item)
        => await _context.CartItems.AddAsync(item);

    public void UpdateCartItem(CartItem item)
        => _context.CartItems.Update(item);

    public void RemoveCartItem(CartItem item)
        => _context.CartItems.Remove(item);

    public async Task ClearCartAsync(int cartId)
    {
        var items = await _context.CartItems
            .Where(ci => ci.CartId == cartId)
            .ToListAsync();
        _context.CartItems.RemoveRange(items);
    }
}