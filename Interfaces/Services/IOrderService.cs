using Giftify.Models;
using Giftify.ViewModels.Order;

namespace Giftify.Interfaces.Services;

public interface IOrderService
{
    /// <summary>Converts the user's current cart into an Order and saves it.</summary>
    Task<Order> PlaceOrderAsync(string userId, CheckoutVM checkout);

    /// <summary>Returns all orders for a user, newest first.</summary>
    Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId);

    /// <summary>Updates the status of an existing order (Admin use).</summary>
    Task UpdateOrderStatusAsync(int orderId, string newStatus);
}
