using Giftify.Enums;
using Giftify.Models;

namespace Giftify.Interfaces.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{
    
    Task<Order?> GetOrderWithDetailsAsync(int orderId);


    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);

    Task<IEnumerable<Order>> GetOrdersByUserIdAndStatusAsync(string userId, string status);

    Task<IEnumerable<Order>> GetAllOrdersAsync();

    Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);


    Task<decimal> GetTotalRevenueAsync();

    Task<int> GetOrderCountSinceAsync(DateTime since);
}