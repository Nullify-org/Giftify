using Giftify.Interfaces;
using Giftify.Interfaces.Services;
using Giftify.Models;
using Giftify.ViewModels.Order;

namespace Giftify.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartService _cartService;

    public OrderService(IUnitOfWork unitOfWork, ICartService cartService)
    {
        _unitOfWork  = unitOfWork;
        _cartService = cartService;
    }

    // ── PlaceOrderAsync ──────────────────────────────────────────────────────

    public async Task<Order> PlaceOrderAsync(string userId, CheckoutVM checkout)
    {
        // 1. Load the user's cart
        var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);

        if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            throw new InvalidOperationException("Your cart is empty.");

        // 2. Validate stock for every item and build OrderItems
        var orderItems = new List<OrderItem>();

        foreach (var ci in cart.CartItems)
        {
            var product = await _unitOfWork.Products
                .FindAsync(p => p.Id == ci.ProductId);

            if (product == null)
                throw new InvalidOperationException($"Product {ci.ProductId} no longer exists.");

            if (product.Stock < ci.Quantity)
                throw new InvalidOperationException(
                    $"Only {product.Stock} unit(s) of \"{product.Name}\" available.");

            // Deduct stock
            product.Stock -= ci.Quantity;
            _unitOfWork.Products.Update(product);

            orderItems.Add(new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity  = ci.Quantity,
                UnitPrice = ci.UnitPrice   // price snapshot from cart
            });
        }

        // 3. Build the Order
        var shippingAddress = $"{checkout.FullName}, {checkout.Street}, {checkout.City}" +
                              (!string.IsNullOrWhiteSpace(checkout.State)      ? $", {checkout.State}"      : "") +
                              (!string.IsNullOrWhiteSpace(checkout.PostalCode) ? $" {checkout.PostalCode}"  : "") +
                              $" — Phone: {checkout.PhoneNumber}";

        var order = new Order
        {
            UserId          = userId,
            ShippingAddress = shippingAddress,
            OrderDate       = DateTime.Now,
            Status          = "Pending",
            GiftMessage     = checkout.GiftMessage,
            TotalAmount     = orderItems.Sum(i => i.UnitPrice * i.Quantity),
            OrderItems      = orderItems
        };

        await _unitOfWork.Orders.AddAsync(order);

        // 4. Clear the cart
        await _unitOfWork.Carts.ClearCartItemsAsync(cart.Id);

        // 5. Commit everything in one transaction
        await _unitOfWork.CompleteAsync();

        return order;
    }

    // ── GetOrdersByUserAsync ─────────────────────────────────────────────────

    public async Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId)
    {
        return await _unitOfWork.Orders.GetOrdersByUserAsync(userId);
    }

    // ── UpdateOrderStatusAsync ───────────────────────────────────────────────

    public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
    {
        await _unitOfWork.Orders.UpdateOrderStatusAsync(orderId, newStatus);
        await _unitOfWork.CompleteAsync();
    }
}
