namespace Giftify.Models;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string ShippingAddress { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string GiftMessage { get; set; }
    // as one user  has one to many relation with orders
    public ApplicationUser User { get; set; }
    //as it has one to many relation with order items
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public Payment? Payment { get; set; }
}
