using Giftify.ViewModels.Cart;

namespace Giftify.Interfaces.Services;

public interface ICartService
{
    Task<CartVM> GetCartAsync(string userId);
    Task AddToCartAsync(string userId, int productId, int quantity = 1);
    Task UpdateQuantityAsync(string userId, int productId, int quantity);
    Task RemoveFromCartAsync(string userId, int productId);
    Task ClearCartAsync(string userId);
    Task<int> GetCartItemCountAsync(string userId);
}

