namespace Giftify.ViewModels.Cart;

public class CartVM
{
    public int CartId { get; set; }
    public string UserId { get; set; }
    public List<CartItemVM> Items { get; set; } = new();
    public int TotalItems => Items.Sum(i => i.Quantity);
    public decimal Subtotal => Items.Sum(i => i.LineTotal);
    public decimal ShippingEstimate => Subtotal >= 500 ? 0 : 50;
    public decimal GrandTotal => Subtotal + ShippingEstimate;
    public bool IsEmpty => !Items.Any();
}
