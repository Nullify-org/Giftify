namespace Giftify.Models;

public class Order
{
    public int Id { get; set; }
    public string ShippingAddress { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public int TotalAmount { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public string GiftMessage { get; set; }
}
