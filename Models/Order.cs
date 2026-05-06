namespace Giftify.Models;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string ShippingAddress { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public string Status { get; set; }
    public int TotalAmount { get; set; }
    public string GiftMessage { get; set; }
    public ApplicationUser User { get; set; }
    public ICollection<OrderItem> orderItems { get; set; } = new List<OrderItem>();
}
