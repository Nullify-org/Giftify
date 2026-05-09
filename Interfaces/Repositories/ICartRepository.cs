using Giftify.Models;

namespace Giftify.Interfaces.Repositories;

public interface ICartRepository : IBaseRepository<Cart>
{
    Task<Cart?> GetCartByUserIdAsync(string userId);
    Task<CartItem?> GetCartItemAsync(int cartId, int productId);
    Task AddCartItemAsync(CartItem item);
    void UpdateCartItem(CartItem item);
    void RemoveCartItem(CartItem item);
    Task ClearCartAsync(int cartId);
}