using Giftify.Models;

namespace Giftify.Interfaces.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{
  
    Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId);

  
    Task<Order?> GetOrderWithDetailsAsync(int orderId);

 
    Task<IEnumerable<Order>> GetAllOrdersWithUsersAsync();

    
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);

    Task UpdateOrderStatusAsync(int orderId, string newStatus);
}
