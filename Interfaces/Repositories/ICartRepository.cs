using Giftify.Models;

namespace Giftify.Interfaces.Repositories;

public interface ICartRepository : IBaseRepository<Cart>
{
   
    Task<Cart?> GetCartWithItemsAsync(string userId);

    Task<CartItem?> GetCartItemAsync(int cartItemId);

    Task AddCartItemAsync(CartItem item);

    void UpdateCartItem(CartItem item);

    void RemoveCartItem(CartItem item);

    Task ClearCartItemsAsync(int cartId);
}
