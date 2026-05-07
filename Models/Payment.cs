namespace Giftify.Models;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public string? TransactionReference { get; set; }
    public int TransactionId { get; set; }
    public DateTime PaidAt { get; set; }
    public Order Order { get; set; }
}
