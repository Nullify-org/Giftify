using Giftify.ViewModels.Cart;

namespace Giftify.Interfaces.Services;

public interface ICartService
{
    Task<CartVM> GetCartForUserAsync(string userId);

   
    Task AddItemAsync(string userId, AddToCartVM request);

    Task UpdateItemQuantityAsync(string userId, UpdateCartItemVM request);

    Task RemoveItemAsync(string userId, int cartItemId);

    Task ClearCartAsync(string userId);

    Task<int> GetCartItemCountAsync(string userId);
}
